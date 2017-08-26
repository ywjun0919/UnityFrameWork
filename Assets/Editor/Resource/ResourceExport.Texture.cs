using System.Collections.Generic;
using UnityEditor;
using Game;
using UnityEngine;

public partial class ResourceExporter
{
    static Dictionary<string, string> m_assetTextures = new Dictionary<string, string>();

    static void GetTexturesAssets()
    {
        m_assetTextures.Clear();
        GetAssetsRecursively(GameSetting.assetPath + "texture/", "*.png", "texture/", "", ref m_assetTextures);
        GetAssetsRecursively(GameSetting.assetPath + "texture/", "*.tga", "texture/", "", ref m_assetTextures);
        GetAssetsRecursively(GameSetting.assetPath + "texture/", "*.exr", "texture/", "", ref m_assetTextures);
    }
    static void ExportSelectedTextures(BuildTarget target)
    {
        UnityEngine.Object[] selection = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
        if (selection.Length > 0)
        {
            GetTexturesAssets();
            var assets = GetSelectedAssets(m_assetTextures, selection);
            SetAssetBundleName(assets);
            BuildAssetBundles(target);
        }

    }

    static void ExportAllTextures(BuildTarget target)
    {
        GetTexturesAssets();
        SetAssetBundleName(m_assetTextures);
        BuildAssetBundles(target);

    }
}