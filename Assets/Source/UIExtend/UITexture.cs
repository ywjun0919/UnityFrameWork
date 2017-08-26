

using Game;

namespace UnityEngine.UI
{
    class UITexture : RawImage
    {
        AssetInfo m_AssetInfo = null;
        string textureName = string.Empty;
        public void SetTexture(AssetInfo assetInfo)
        {
            if (bTextureEqual(assetInfo))
            {
                return;
            }

            if (assetInfo != null)
            {
                Resource.Load(assetInfo, (obj) =>
                {
                    AssetBundle ab = obj as AssetBundle;
                    var objs = ab.LoadAllAssets();
                    Texture t = objs.Length > 0 ? objs[0] as Texture : null;
                    this.texture = t;
                });
            }
            FreeTextureRef();
            this.m_AssetInfo = assetInfo;
        }

        private bool bTextureEqual(AssetInfo ai)
        {
            bool bEqual = (null != m_AssetInfo && null != ai && ai.assetPath != m_AssetInfo.assetPath) ||
                (null == m_AssetInfo && null == ai);
            return bEqual;
        }

        private void FreeTextureRef()
        {
            if (null != m_AssetInfo)
                this.m_AssetInfo.RefCachePolicy.Free(this.m_AssetInfo);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            FreeTextureRef();
        }
    }
}
