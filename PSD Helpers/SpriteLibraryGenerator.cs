using System.Collections.Generic;
using UnityEditor;
using UnityEditor.U2D;
using UnityEditor.U2D.PSD;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace PSDHelpers
{
    public static class SpriteLibraryGenerator
    {
        class LayerData
        {
            public string name;
            public HashSet<LayerData> children = new HashSet<LayerData>();
            public Sprite sprite;
            public bool isGroup;
        }

        public static SpriteLibraryAsset ConvertGroupsToCategories(PSDImporter importer, IList<Sprite> sprites)
        {
            var so = new SerializedObject(importer);
            var psdLayers = so.FindProperty("m_PsdLayers");

            var layerData = new List<LayerData>();
            for (var i = 0; i < psdLayers.arraySize; ++i)
            {
                var layer = psdLayers.GetArrayElementAtIndex(i);
                var layerName = layer.FindPropertyRelative("m_Name").stringValue;
                var spriteId = layer.FindPropertyRelative("m_SpriteID").stringValue;
                var isGroup = layer.FindPropertyRelative("m_IsGroup").boolValue;
                var sprite = GetSpriteByGUID(sprites, spriteId);

                layerData.Add(new LayerData
                {
                    name = layerName,
                    sprite = sprite,
                    isGroup = isGroup
                });
            }

            var roots = new List<LayerData>();
            for (var i = 0; i < psdLayers.arraySize; ++i)
            {
                var layer = psdLayers.GetArrayElementAtIndex(i);
                var parentId = layer.FindPropertyRelative("m_ParentIndex").intValue;

                if (parentId < 0)
                    roots.Add(layerData[i]);
                else
                    layerData[parentId].children.Add(layerData[i]);
            }

            var lib = ScriptableObject.CreateInstance<SpriteLibraryAsset>();
            foreach (var root in roots)
            {
                var openSet = new Queue<LayerData>();
                openSet.Enqueue(root);
                foreach (var child in root.children)
                    openSet.Enqueue(child);

                var children = new List<LayerData>();
                while (openSet.Count > 0)
                {
                    var child = openSet.Dequeue();
                    {
                        if (!child.isGroup)
                            children.Add(child);
                    }
                    foreach (var nestedChild in child.children)
                        openSet.Enqueue(nestedChild);
                }

                foreach (var child in children)
                    lib.AddCategoryLabel(child.sprite, root.name, child.name);
            }

            return lib;

        }

        public static SpriteLibraryAsset ConvertLayersToCategories(PSDImporter importer, IList<Sprite> sprites)
        {
            var lib = ScriptableObject.CreateInstance<SpriteLibraryAsset>();
            var so = new SerializedObject(importer);
            var psdLayers = so.FindProperty("m_PsdLayers");
            for (var i = 0; i < psdLayers.arraySize; ++i)
            {
                var layer = psdLayers.GetArrayElementAtIndex(i);
                if (layer.FindPropertyRelative("m_IsGroup").boolValue)
                    continue;

                var layerName = layer.FindPropertyRelative("m_Name").stringValue;
                var spriteId = layer.FindPropertyRelative("m_SpriteID").stringValue;
                var sprite = GetSpriteByGUID(sprites, spriteId);

                lib.AddCategoryLabel(sprite, layerName, layerName);
            }

            return lib;
        }

        static Sprite GetSpriteByGUID(IList<Sprite> sprites, string guid)
        {
            foreach (var sprite in sprites)
            {
                if (sprite.GetSpriteID().ToString() == guid)
                    return sprite;
            }

            return null;
        }
    }
}
