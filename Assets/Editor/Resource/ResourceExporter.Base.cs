using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

public partial class ResourceExporter
{
    static void GetAssetsRecursively(string srcFolder, string searchPattern, string dstFolder, string prefix, ref Dictionary<string, string> assets)
    {
        GetAssetsRecursively(srcFolder, searchPattern, dstFolder, prefix, "bundle", ref assets);
    }

    /// <summary>
    ///  Get assets for audio, texture, etc
    ///  循环导出子目录中所有符合searchPattern后缀的文件
    /// </summary>
    static void GetAssetsRecursively(string srcFolder, string searchPattern, string dstFolder, string prefix, string suffix, ref Dictionary<string, string> assets)
    {
        //UnityEngine.Debug.Log("GeneratePath: " + srcFolder);

        string searchFolder = StandardlizePath(srcFolder);
        if (!Directory.Exists(searchFolder))
            return;

        string srcDir = searchFolder;
        string[] files = Directory.GetFiles(srcDir, searchPattern);
        foreach (string oneFile in files)
        {
            string srcFile = StandardlizePath(oneFile);
            if (!File.Exists(srcFile))
                continue;

            string dstFile;
            if (string.IsNullOrEmpty(prefix))
            {
                dstFile = Path.Combine(dstFolder, string.Format("{0}.{1}", Path.GetFileNameWithoutExtension(srcFile), suffix));
            }
            else
            {
                dstFile = Path.Combine(dstFolder, string.Format("{0}_{1}.{2}", prefix, Path.GetFileNameWithoutExtension(srcFile), suffix));
            }
            dstFile = StandardlizePath(dstFile);
            assets[srcFile] = dstFile;

            //UnityEngine.Debug.Log("srcFile: " + srcFile + " => dstFile: " + dstFile);
        }

        string[] dirs = Directory.GetDirectories(searchFolder);
        foreach (string oneDir in dirs)
        {
            GetAssetsRecursively(oneDir, searchPattern, dstFolder, prefix, suffix, ref assets);
        }
    }


    static string StandardlizePath(UnityEngine.Object obj)
    {
        return StandardlizePath(AssetDatabase.GetAssetPath(obj));
    }

    public static string StandardlizePath(string path)
    {
        string pathReplace = path.Replace(@"\", @"/");
        string pathLower = pathReplace.ToLower();
        return pathLower;
    }

    static void ResetAssetBundleNames()
    {
        int length = AssetDatabase.GetAllAssetBundleNames().Length;

        string[] oldAssetBundleNames = new string[length];
        for (int i = 0; i < length; i++)
        {
            oldAssetBundleNames[i] = AssetDatabase.GetAllAssetBundleNames()[i];
        }

        for (int j = 0; j < oldAssetBundleNames.Length; j++)
        {
            AssetDatabase.RemoveAssetBundleName(oldAssetBundleNames[j], true);
        }
    }
    static void SetAssetBundleName(Dictionary<string, string> assets)
    {
        ResetAssetBundleNames();

        AssetImporter importer = null;
        foreach (KeyValuePair<string, string> pair in assets)
        {
            importer = AssetImporter.GetAtPath(pair.Key);
            if (importer == null)
            {
                continue;
            }
            if (importer.assetBundleName == null || importer.assetBundleName != pair.Value)
            {
                importer.assetBundleName = pair.Value;
            }
        }
    }
    static void CombineAssets(Dictionary<string, string>[] dics, ref Dictionary<string, string> assets)
    {
        for (int i = 0; i < dics.Length; ++i)
        {
            Dictionary<string, string> dic = dics[i];
            foreach (KeyValuePair<string, string> pair in dic)
            {
                assets[pair.Key] = pair.Value;
            }
        }
    }
    static void WriteDependenceConfig(BuildTarget target, Dictionary<string, List<string>> m_Dependencies)
    {
        string fileName = GetBundleSavePath(target, "config/dependency");
        string jsonFileName = GetBundleSavePath(target, "config/dependency.json");
        Directory.CreateDirectory(Path.GetDirectoryName(fileName));

        FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
        FileStream f = new FileStream(jsonFileName, FileMode.Create, FileAccess.ReadWrite);
        f.Close();

        JsonSerialization.ToFile(GetBundleSavePath(target, "config/dependency.json"), m_Dependencies);

        BinaryWriter w = new BinaryWriter(fs);
        w.Write(m_Dependencies.Count);

        foreach (KeyValuePair<string, List<string>> pair in m_Dependencies)
        {
            w.Write(pair.Key);
            w.Write(pair.Value.Count);
            foreach (string s in pair.Value)
            {
                w.Write(s);
            }
        }
        w.Close();
        fs.Close();

        if (true)
        {
            using (StreamWriter sw = File.CreateText(fileName + "text"))
            {
                sw.WriteLine("size = " + m_Dependencies.Count);

                foreach (KeyValuePair<string, List<string>> pair in m_Dependencies)
                {
                    sw.WriteLine(pair.Key);
                    sw.WriteLine(pair.Value.Count);
                    foreach (string s in pair.Value)
                    {
                        sw.WriteLine(s);
                    }
                }
                sw.Close();
            }

        }

    }
}