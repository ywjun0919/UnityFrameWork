


using Game;
using System.Collections.Generic;
using UnityEngine;

public class Loader
{
    private static Dictionary<string, List<string>> m_AllDependency = null;
    public static Dictionary<string, List<string>> AllDependency {
        get {
            return LoadAllDependency();
        }
    }
    private static Dictionary<string, List<string>> LoadAllDependency()
    {
        if (null == m_AllDependency)
        {
            m_AllDependency = new Dictionary<string, List<string>>();
            string path = Game.ApplicationPath.DepencyPath;
            JsonSerialization.FromFile<Dictionary<string, List<string>>>(path, out m_AllDependency);
        }
        return m_AllDependency; 
    }

    public static void LoadUI(string uiPath,System.Action<GameObject> callBack)
    {
        if (m_Objs.ContainsKey(uiPath))
            return;
        AssetInfo assetInfo = CreateAssetInfo(uiPath);
        Resource.Load(assetInfo, (obj) =>
        {
            AssetBundle ab = (AssetBundle)obj;
            GameObject newObj =(GameObject) Object.Instantiate(ab.LoadAllAssets()[0]);
            Attachment.Create(newObj, assetInfo);
            m_Objs.Add(uiPath, newObj);
            if(null != callBack)
            {
                callBack(newObj);
            }
        });
    }

    public static void Destory(string uiPath)
    {
        if(m_Objs.ContainsKey(uiPath))
        {
            GameObject.Destroy(m_Objs[uiPath]);
            m_Objs.Remove(uiPath);
        }
    }

    //Test
    public static Dictionary<string, GameObject> m_Objs = new Dictionary<string, GameObject>();
    public static AssetInfo CreateAssetInfo(string assetPath)
    {
        AssetInfo assetInfo = new AssetInfo();
        assetInfo.assetPath = ApplicationPath.GetPath(assetPath);
        assetInfo.RefCachePolicy = Cache.GetOrCreateCache("ui");
        assetInfo.RefDirectDeps = new List<AssetInfo>();
        assetInfo.locationType = AssetLocation.Local;
        assetInfo.assetType = AssetType.assetbundle;

        List<string> depPaths = AllDependency.Get(assetPath);
        if(null != depPaths)
        {
            foreach (var path in depPaths)
            {
               assetInfo.RefDirectDeps.Add(CreateAssetInfo(path));
            }
        }
       
        return assetInfo;
    }
}
