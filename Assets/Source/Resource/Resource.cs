
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Resource
    {
        private static bool bLoadInEditor = false;
        private static Dictionary<string, Cache> m_CachePolicy = new Dictionary<string, Cache>();
        private static Dictionary<string, List<Action<UnityEngine.Object>>> m_LodingCallBackDic = new Dictionary<string, List<Action<UnityEngine.Object>>>();
        private static string GetResPath(string assetPath)
        {
            return assetPath;
        }

        public static void LoadAssetFromLocation(AssetInfo assetInfo, Action<UnityEngine.Object> callBack)
        {
            AssetLocation loadType = assetInfo.locationType;
            switch (loadType)
            {
                case AssetLocation.Resources:
                    GameMain.Coroutine(CoroutineLoadRes(assetInfo, callBack)); break;
                case AssetLocation.WWW:
                    GameMain.Coroutine(CoroutineLoadUseWWW(assetInfo, callBack)); break;
                default:
                    if (assetInfo.assetType == AssetType.assetbundle)
                        GameMain.Coroutine(CoroutineLoadAB(assetInfo, callBack));
                    break;
            }

        }

        private static void LoadAssetAtAB(AssetInfo assetInfo, AssetBundle ab, Action<UnityEngine.Object> callBack)
        {
            if (null == ab)
            {
                AssertCallBack(null, callBack, "AssetBundle Error");
                return;
            }
            CoroutineLoadAtAB(assetInfo, ab, callBack);
        }

        private static IEnumerator CoroutineLoadUseWWW(AssetInfo assetInfo, Action<UnityEngine.Object> callBack)
        {
            var path = GetResPath(assetInfo.assetPath);
            WWW www = new WWW(path);
            yield return www;
            var error = www.error;
            if (null == error)
            {
                var ab = www.assetBundle;
                AssertCallBack(ab, callBack, "Resources has no asset" + assetInfo.assetPath);
            }
            else
            {
                AssertCallBack(null, callBack, string.Format("www error {0},path {1},err {2}", assetInfo.assetPath, path, error));
            }
        }

        private static IEnumerator CoroutineLoadRes(AssetInfo assetInfo, Action<UnityEngine.Object> callBack)
        {
            var req = Resources.LoadAsync(assetInfo.assetPath);
            yield return req;
            var asset = req.asset;
            AssertCallBack(asset, callBack, "Resources has no asset" + assetInfo.assetPath);
        }

        private static IEnumerator CoroutineLoadAB(AssetInfo assetInfo, Action<UnityEngine.Object> callBack)
        {
            var path = GetResPath(assetInfo.assetPath);
            var assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(path);
            yield return assetBundleCreateRequest;
            var ab = assetBundleCreateRequest.assetBundle;
            AssertCallBack(ab, callBack, "LoadFromFileAsync.assetBundle nil " + assetInfo.assetPath);
        }

        private static IEnumerable CoroutineLoadAtAB(AssetInfo assetInfo, AssetBundle ab, Action<UnityEngine.Object> callBack)
        {
            AssetBundleRequest req = null;
            if (assetInfo.assetType != AssetType.sprite)
            {
                req = ab.LoadAssetAsync(assetInfo.assetPath);
            }
            else
            {
                req = ab.LoadAssetAsync<UnityEngine.Sprite>(assetInfo.assetPath);
            }
            yield return req;
            var asset = req.asset;
            AssertCallBack(asset, callBack, "LoadAssetAsync err  " + assetInfo.assetPath);
        }

        private static void AssertCallBack(UnityEngine.Object obj, Action<UnityEngine.Object> callBack, string errorInfo)
        {
            if (null == obj)
            {
                Debug.LogError(errorInfo);
            }
            if (null != callBack)
            {
                callBack(obj);
            }
            else
            {
                Debug.LogWarning("Load AssetInfo Has not callBack");
            }
        }

        public void Init(Dictionary<string, int> assetcachepolicys)
        {
            foreach (var policy in assetcachepolicys)
            {
                var cacheName = policy.Key;
                var cache = new Cache(cacheName, policy.Value);
                m_CachePolicy.Add(cacheName, cache);
            }
        }
        public static void Load(AssetInfo assetInfo, Action<UnityEngine.Object> resLoadCallback)
        {
            DoLoad(assetInfo,resLoadCallback);
        }

        private static void DoLoad(AssetInfo assetInfo, Action<UnityEngine.Object> resLoadCallback)
        {
            var cachedAsset = assetInfo.RefCachePolicy.Get(assetInfo);
            if (null != cachedAsset)
            {
                resLoadCallback(cachedAsset.asset);
                return;
            }
            var assetPath = assetInfo.assetPath;
            List<Action<UnityEngine.Object>> cbs = null;
            if (m_LodingCallBackDic.TryGetValue(assetPath, out cbs))
            {
                cbs.Add(resLoadCallback);
                return;
            }
            cbs = new List<Action<UnityEngine.Object>>();
            cbs.Add(resLoadCallback);
            m_LodingCallBackDic.Add(assetPath, cbs);
            if (assetInfo.locationType == AssetLocation.Resources)
            {
                LoadAssetFromLocation(assetInfo, (obj) => { PutCacheThenCallback(assetInfo, obj); });
            }
            else if (assetInfo.assetType == AssetType.assetbundle)
            {
                int depcnt = assetInfo.RefDirectDeps.Count;
                if (0 == depcnt)
                {
                    LoadAssetFromLocation(assetInfo, (obj) => { PutCacheThenCallback(assetInfo, obj); });
                }
                else
                {
                    int loadingCnt = 0;
                    for (int i = 0; i < depcnt; ++i)
                    {
                        DoLoad(assetInfo.RefDirectDeps[i], (obj) =>
                        {
                            loadingCnt = loadingCnt + 1;
                            if (loadingCnt == depcnt)
                                LoadAssetFromLocation(assetInfo, (obj1) => { PutCacheThenCallback(assetInfo, obj1); });
                        });
                    }
                }
            }
            else
            {
                AssetInfo abassetinfo = assetInfo.RefDirectDeps[1];
                DoLoad(abassetinfo, (obj) =>
                {
                    LoadAssetAtAB(assetInfo, obj as AssetBundle, (obj1) => { PutCacheThenCallback(assetInfo, obj1); });
                });
            }

        }

        private static void PutCacheThenCallback(AssetInfo assetInfo, UnityEngine.Object obj)
        {
            List<Action<UnityEngine.Object>> cbs = null;
            if (m_LodingCallBackDic.TryGetValue(assetInfo.assetPath, out cbs))
            {
                int count = cbs.Count;
                assetInfo.RefCachePolicy.Put(assetInfo, obj,count);
                m_LodingCallBackDic.Remove(assetInfo.assetPath);
                foreach(var cb in cbs)
                {
                    cb(obj);
                }
            }
        }

        public static void Free(AssetInfo assetInfo)
        {
            assetInfo.RefCachePolicy.Free(assetInfo);
        }

        public static void RealFree(AssetInfo assetInfo)
        {
            foreach(var dep in assetInfo.RefDirectDeps)
            {
                Free(assetInfo);
            }
        }

    }

    public enum AssetLocation
    {
        Resources,
        WWW,
        Local,
    }

    public enum AssetType
    {
        assetbundle = 1,
        asset = 2,
        prefab = 3,
        sprite = 4,
    }

    public class AssetInfo
    {
        public string assetPath = string.Empty;
        public AssetLocation locationType = AssetLocation.WWW;
        public AssetType assetType = AssetType.prefab;
        public List<AssetInfo> RefDirectDeps = new List<AssetInfo>();
        public string cacheName = string.Empty;
        public Cache RefCachePolicy = null;
    }
}
