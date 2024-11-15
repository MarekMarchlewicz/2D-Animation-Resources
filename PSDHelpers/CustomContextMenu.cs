using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.U2D.Animation;
using UnityEditor.U2D.PSD;
using UnityEngine;

namespace PSDHelpers
{
    static class CustomContextMenu
    {
        [MenuItem("Assets/PSD/Convert Groups To Categories", false, 1000)]
        static void ConvertGroupsToCategories()
        {
            var importer = GetImporterFromSelection();
            if (importer == null)
                return;

            var subAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(importer.assetPath);
            var sprites = new List<Sprite>();
            foreach (var subAsset in subAssets)
            {
                if (subAsset is Sprite sprite)
                    sprites.Add(sprite);
            }

            var generatedLibrary = SpriteLibraryGenerator.ConvertGroupsToCategories(importer, sprites);
            if (generatedLibrary == null)
                return;
            var savePath = AssetDatabase.GenerateUniqueAssetPath(Path.ChangeExtension(importer.assetPath, ".spriteLib"));
            var path = generatedLibrary.SaveAsSourceAsset(savePath);
            AssetDatabase.ImportAsset(path);
        }

        [MenuItem("Assets/PSD/Convert Groups To Categories", true)]
        static bool ConvertGroupsToCategoriesValidation()
        {
            return GetImporterFromSelection() != null;
        }

        [MenuItem("Assets/PSD/Convert Layers To Categories", false, 1000)]
        static void ConvertLayersToCategories()
        {
            var importer = GetImporterFromSelection();
            if (importer == null)
                return;

            var subAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(importer.assetPath);
            var sprites = new List<Sprite>();
            foreach (var subAsset in subAssets)
            {
                if (subAsset is Sprite sprite)
                    sprites.Add(sprite);
            }

            var generatedLibrary = SpriteLibraryGenerator.ConvertLayersToCategories(importer, sprites);

            var savePath = AssetDatabase.GenerateUniqueAssetPath(Path.ChangeExtension(importer.assetPath, ".spriteLib"));
            var path = generatedLibrary.SaveAsSourceAsset(savePath);
            AssetDatabase.ImportAsset(path);
        }

        [MenuItem("Assets/PSD/Convert Layers To Categories", true)]
        static bool ConvertLayersToCategoriesValidation()
        {
            return GetImporterFromSelection() != null;
        }

        static PSDImporter GetImporterFromSelection()
        {
            if (Selection.activeObject == null)
                return null;

            var assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(assetPath))
                return null;

            var importer = AssetImporter.GetAtPath(assetPath);
            if (importer == null || importer is PSDImporter == false)
                return null;

            return (PSDImporter)AssetImporter.GetAtPath(assetPath);
        }
    }
}
