using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public partial class ResourceExporter
{
    public static string GetBundleSavePath(BuildTarget target, string relativePath)
    {
        string path;
        switch (target)
        {
            case BuildTarget.Android:
            case BuildTarget.iOS:
                path = string.Format(Game.GameSetting.relativePath+"{2}", Application.dataPath, GetPlatfomrPath(target), relativePath);
                break;
            default:
                path = string.Format(Game.GameSetting.relativePath+"/Data/{2}", Application.dataPath, GetPlatfomrPath(target), relativePath);
                break;
        }
        return path;
    }

    static void BuildAssetBundles(BuildTarget target, BuildAssetBundleOptions options = BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression)
    {
        string dir = GetBundleSaveDir(target);
        Directory.CreateDirectory(Path.GetDirectoryName(dir));

        BuildPipeline.BuildAssetBundles(dir, options, target);

        ResetAssetBundleNames();
        SaveDependency(target);
    }

    static void SaveDependency(BuildTarget target)
    {
        string dir = GetBundleSaveDir(target);
        string depfile = dir.Substring(dir.TrimEnd('/').LastIndexOf("/") + 1);
        depfile = depfile.TrimEnd('/');
        string path = GetBundleSavePath(target, depfile);

        AssetBundle ab = AssetBundle.LoadFromFile(path);

        AssetBundleManifest manifest = (AssetBundleManifest)ab.LoadAsset("AssetBundleManifest");

        ab.Unload(false);

        Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();

        LoadOldDependency(target, dic);

        foreach (string asset in manifest.GetAllAssetBundles())
        {
            List<string> list = new List<string>();
            string[] deps = manifest.GetDirectDependencies(asset);
            foreach (string dep in deps)
            {
                list.Add(dep);
            }
            if (deps.Length > 0)
                dic[asset] = list;
            else if (dic.ContainsKey(asset))
                dic.Remove(asset);
        }

        WriteDependenceConfig(target, dic);
    }

    static void LoadOldDependency(BuildTarget target, Dictionary<string, List<string>> dic)
    {
        string dataPath = GetBundleSavePath(target, "config/dependency");
        if (!File.Exists(dataPath))
        {
            return;
        }

        FileStream fs = new FileStream(dataPath, FileMode.Open, FileAccess.Read);
        BinaryReader br = new BinaryReader(fs);

        int size = br.ReadInt32();
        string resname;
        string textureBundleName;

        for (int i = 0; i < size; i++)
        {
            resname = br.ReadString();
            int count = br.ReadInt32();
            if (!dic.ContainsKey(resname))
                dic[resname] = new List<string>();
            //Debug.Log(sfxname + "  " + count);
            for (int j = 0; j < count; ++j)
            {
                textureBundleName = br.ReadString();
                dic[resname].Add(textureBundleName);
            }
        }
        br.Close();
        fs.Close();
        var dataPath2 = GetBundleSavePath(target, "config/dependency.json");
        if (File.Exists(dataPath2))
        {
            Dictionary<string, List<string>> dicJs = new Dictionary<string, List<string>>();
            var re = JsonSerialization.FromFile(dataPath2, out dicJs);
            if (re == true)
            {
                dic.Clear();
                dic.AddRange(dicJs);
            }
            else
            {
                Debug.LogError("Json config/dependency.json load error");
            }
        }
    }
}
