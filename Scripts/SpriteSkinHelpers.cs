using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.U2D.Animation;

public static class AnimationHelpers
{
    const string k_AnimationRuntimeNamespace = "UnityEngine.U2D.Animation";
    const string k_AnimationRuntimeAssembly = "Unity.2D.Animation.Runtime";

    static readonly Assembly k_RuntimeAssembly = Assembly.Load(k_AnimationRuntimeAssembly);
    static readonly Type k_SpriteSkinType = k_RuntimeAssembly.GetType($"{k_AnimationRuntimeNamespace}.SpriteSkin");
    static readonly PropertyInfo k_SpriteSkinGetAutoRebind = k_SpriteSkinType.GetProperty("autoRebind", BindingFlags.Instance | BindingFlags.NonPublic);
    static readonly PropertyInfo k_SpriteSkinIsValid = k_SpriteSkinType.GetProperty("isValid", BindingFlags.Instance | BindingFlags.NonPublic);
    static readonly PropertyInfo k_SpriteSkinRootBone = k_SpriteSkinType.GetProperty("rootBone", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    static readonly PropertyInfo k_SpriteSkinBoneTransforms = k_SpriteSkinType.GetProperty("boneTransforms", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

    /// <summary>
    /// Gets the value of Auto Rebind property.
    /// </summary>
    /// <param name="spriteSkin">Sprite Skin component.</param>
    /// <returns>True if the Auto Rebind is enabled, otherwise false.</returns>
    public static bool GetAutoRebind(this SpriteSkin spriteSkin) => (bool)k_SpriteSkinGetAutoRebind.GetValue(spriteSkin);

    /// <summary>
    /// Sets the value of Auto Rebind property.
    /// </summary>
    /// <param name="spriteSkin">Sprite Skin component.</param>
    /// <param name="autoRebind">True if the Auto Rebind is enabled, otherwise false.</param>
    public static void SetAutoRebind(this SpriteSkin spriteSkin, bool autoRebind) => k_SpriteSkinGetAutoRebind.SetValue(spriteSkin, autoRebind);

    /// <summary>
    /// Sets the Transform Component that represents the root bone for deformation.
    /// </summary>
    /// <param name="spriteSkin">Sprite Skin component.</param>
    /// <param name="rootBone">A Transform Component of the root bone.</param>
    /// <returns>True if successful.</returns>
    public static bool SetRootBone(this SpriteSkin spriteSkin, Transform rootBone)
    {
        k_SpriteSkinRootBone.SetValue(spriteSkin, rootBone);
        return (bool)k_SpriteSkinIsValid.GetValue(spriteSkin);
    }

    /// <summary>
    /// Sets the Transform Components that are used for deformation.
    /// </summary>
    /// <param name="spriteSkin">Sprite Skin component.</param>
    /// <param name="boneTransformsArray">Array of new bone Transforms.</param>
    /// <returns>True if successful.</returns>
    public static bool SetBoneTransforms(this SpriteSkin spriteSkin, Transform[] boneTransformsArray)
    {
        k_SpriteSkinBoneTransforms.SetValue(spriteSkin, boneTransformsArray);
        return (bool)k_SpriteSkinIsValid.GetValue(spriteSkin);
    }
}
