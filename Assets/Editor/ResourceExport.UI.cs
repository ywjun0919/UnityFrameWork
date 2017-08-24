using System.Collections.Generic;
using UnityEditor;
using Game;
public partial class ResourceExporter
{
    static string m_UIsPath = GameSetting.assetPath + "ui/Prefabs";
    static Dictionary<string, string> m_assetUIs = new Dictionary<string, string>();
	static string[] m_FontAssetFolder = new string[]
	{
		GameSetting.assetPath+"ui/Font",
	};
	static void GetUIAssets()
	{
        m_assetFonts.Clear ();
		foreach (var folder in m_FontAssetFolder) 
		{
			GetAssetsRecursively (folder, "*.TTF", "ui/font/",null, "ui", ref m_assetFonts);
		}
        GetAssetsRecursively(m_UIsPath, "*.prefab", "ui/", null, "ui", ref m_assetUIs);
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

        CombineAssets(new Dictionary<string, string>[] {  m_assetFonts,}, ref assets);
        //PreProcessAtlas(target);
        //SetAssetBundleName(assets, new string[] { ".shader" }, "shaders/");
		GetUIAssets();

        SetAssetBundleName(assets);
        BuildAssetBundles(target);
        //PostProcessAtlas(target);
    }
}
