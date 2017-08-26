

using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CacheInfo
    {
        public AssetInfo assetInfo;
        public AssetType assetType;
        public UnityEngine.Object asset;
        public int refcnt;
        public int touch;
        public CacheInfo() { }
        public CacheInfo(AssetInfo assetInfo,int refCnt,UnityEngine.Object obj)
        {
            this.assetInfo = assetInfo;
            this.asset = obj;
            this.refcnt = refCnt;
        }
    }

    public class Cache
    {
        private int cacheSize = 10;
        private string cacheName = string.Empty;
        private int serial = 0;
        private Dictionary<string, CacheInfo> m_Loaded = new Dictionary<string, CacheInfo>();
        private Dictionary<string, CacheInfo> m_Cached = new Dictionary<string, CacheInfo>();
        private static Dictionary<string,Cache> m_AllCache = new Dictionary<string,Cache>();
        public Cache(string catheName,int maxSize)
        {
            this.cacheName = catheName;
            this.cacheSize = maxSize;
            m_AllCache.Add(this.cacheName,this);
        }

        public CacheInfo Get(AssetInfo assetInfo)
        {
            CacheInfo cacheInfo = null;
            string assetPath = assetInfo.assetPath;
            m_Loaded.TryGetValue(assetInfo.assetPath, out cacheInfo);
            if (null != cacheInfo)
            {
                cacheInfo.refcnt = cacheInfo.refcnt + 1;
            }
            else
            {
                m_Cached.TryGetValue(assetPath,out cacheInfo);
                if (null != cacheInfo)
                {
                    m_Cached.Remove(assetPath); // 出Cache，进入Loaded
                    m_Loaded.Add(assetPath,cacheInfo);
                    cacheInfo.refcnt = 1;
                }
            }
            return cacheInfo;
        }

        public static Cache GetOrCreateCache(string cacheName)
        {
            if (!m_AllCache.ContainsKey(cacheName))
            {
                new Cache(cacheName,10);
            }
            return m_AllCache[cacheName];

        }

        public void Put(AssetInfo assetInfo,UnityEngine.Object asset,int refCnt)
        {
            CacheInfo ci = null;
            m_Cached.TryGetValue(assetInfo.assetPath, out ci);
            if (null != ci)
            {
                m_Cached.Remove(assetInfo.assetPath);
                Debug.LogError(string.Format("cache.put in cached {0}", assetInfo.assetPath));
            }
            ci = Get(assetInfo);
            if (null != ci)
            {
                Debug.LogError(string.Format("cache.put in cached {0}", assetInfo.assetPath));
            }
            else
            {
                ci = new CacheInfo(assetInfo,refCnt, asset);
                m_Loaded.Add(assetInfo.assetPath, ci);
            }
        }

        public void Free(AssetInfo assetInfo)
        {
            CacheInfo cacheInfo = null;
            var assetPath = assetInfo.assetPath;
            m_Loaded.TryGetValue(assetPath, out cacheInfo);
            if (null != cacheInfo)
            {
                cacheInfo.refcnt -= 1;
                if (cacheInfo.refcnt <= 0)
                {
                    m_Loaded.Remove(assetPath);
                    this.serial = this.serial + 1;
                    cacheInfo.touch = this.serial;
                    this.m_Cached.Add(assetPath, cacheInfo);
                    this.LRUPurge();
                }
            }
        }

        private void LRUPurge()
        {
            string assetPath = string.Empty;
            CacheInfo oldestCache= null;
            FindOldestCacheInfo(ref oldestCache, ref assetPath);
            if (assetPath != string.Empty)
            {
                this.m_Cached.Remove(assetPath);
                var asset = oldestCache.asset;
                var assetInfo = oldestCache.assetInfo;
                var assetType = assetInfo.assetType;
                if (null != asset)
                {
                    switch (assetType)
                    {
                        case AssetType.assetbundle:
                            AssetBundle a = asset as AssetBundle;
                            a.Unload(true);
                            break;
                        case AssetType.prefab: break;
                        default:Resources.UnloadAsset(asset); break;
                    }
                    Resource.RealFree(assetInfo);
                }
            }
        }

        private void FindOldestCacheInfo(ref CacheInfo ci,ref string assetPath)
        {
            if (this.m_Cached.Count > this.cacheSize)
            {
                var e = m_Cached.GetEnumerator();
                while (e.MoveNext())
                {
                    if (ci == null || ci.touch < e.Current.Value.touch)
                    {
                        assetPath = e.Current.Key;
                        ci = e.Current.Value;
                    }
                }
            }
        }

        public int GetRefCnt(AssetInfo assetInfo)
        {
            CacheInfo cacheInfo = null;
            string assetPath = assetInfo.assetPath;
            m_Loaded.TryGetValue(assetInfo.assetPath, out cacheInfo);
            if (null != cacheInfo)
            {
                return cacheInfo.refcnt;
            }
            return 0;
        }
    }
}
