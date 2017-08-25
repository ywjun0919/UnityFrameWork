using System.Collections.Generic;
using UnityEditor;
using Game;
using UnityEngine;

public partial class ResourceExporter
{
    static string m_UIsPath = GameSetting.assetPath + "ui/Prefabs";
    static Dictionary<string, string> m_assetUIs = new Dictionary<string, string>();
    static Dictionary<string, string> m_assetAtlas = new Dictionary<string, string>();

    static string[] m_FontAssetFolder = new string[]
	{
		GameSetting.assetPath+"ui/Font",
	};
    static string[] m_AtlasAssetFolder = new string[]
    {
        "ui/atlas/common",
    };

    static Dictionary<string, string> m_atlas = new Dictionary<string, string>();
    static void GetUIAssets()
	{
        m_assetFonts.Clear ();
		foreach (var folder in m_FontAssetFolder) 
		{
			GetAssetsRecursively (folder, "*.TTF", "ui/font/",null, "ui", ref m_assetFonts);
		}
        GetAtlas();
        GetAssetsRecursively(m_UIsPath, "*.prefab", "ui/", null, "ui", ref m_assetUIs);
    }

    static void GetAtlas()
    {
        m_assetAtlas.Clear();
        foreach (var folder in m_AtlasAssetFolder)
        {
            Dictionary<string, string> temp = new Dictionary<string, string>();
            GetAssetsRecursively(GameSetting.assetPath + folder, "*.png", "ui/atlas/", null, "ui", ref temp);
            Dictionary<string, string> temp1 = new Dictionary<string, string>();
            foreach (var pair in temp)
            {
                temp1.Add(pair.Key,folder+".ui");
            }
            m_assetAtlas.AddRange(temp1);
           // GetAssetsRecursively(folder, "*.asset", "ui/atlas/", null, "ui", ref m_assetAtlas);
        }
    }


        static Dictionary<string, string> GetSelectedAssets(Dictionary<string, string> allassets, UnityEngine.Object[] selection)
    {
        Dictionary<string, string> assets = new Dictionary<string, string>();
        for (int i = 0; i < selection.Length; ++i)
        {
            var assetpath = AssetDatabase.GetAssetPath(selection[i]);
            var assetstandardlizepath = StandardlizePath(selection[i]);
            if (allassets.ContainsKey(assetpath))
            {
                assets[assetpath] = allassets[assetpath];
            }
            else if (allassets.ContainsKey(assetstandardlizepath))
            {
                assets[assetstandardlizepath] = allassets[assetstandardlizepath];
            }
        }
        return assets;
    }

    static Dictionary<string, string> m_assetFonts = new Dictionary<string, string>();

    static void ExportSelectedUIs(BuildTarget target)
    {
        UnityEngine.Object[] selection = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
        if (selection.Length > 0)
        {
            GetUIAssets();
            var assets = GetSelectedAssets(m_assetUIs, selection);
            //GetRedundancyPicInUI(assets);
            BuildAssets(assets, target);
        }
    }
    static void BuildAssets(Dictionary<string, string> assets, BuildTarget target)
    {
        //var _assets = assets.Keys.ToList();
        //ExportNGUI.UIPlaySoundClip2Name(_assets); //清理冗余音效
        //ExportNGUI.UITexture_Texture2Name(_assets);//清理冗余贴图
        //ExportPrefabEvolution.RemoveScript(_assets); //清理无用脚本
        //UnityEditor.UI.BMFontMaker.ReplaceSystemFont(_assets);//替换默认字体

        CombineAssets(new Dictionary<string, string>[] { m_assetAtlas, m_assetFonts,}, ref assets);
        //PreProcessAtlas(target);
        //SetAssetBundleName(assets, new string[] { ".shader" }, "shaders/");
		GetUIAssets();

        SetAssetBundleName(assets);
        BuildAssetBundles(target);
        //PostProcessAtlas(target);
    }
}
