using Game;
using System;


namespace UnityEngine.UI
{
    class UISprite : Image
    {
        private AssetInfo m_AssetInfo = new AssetInfo();
        private AssetInfo m_AltasAssetInfo = null;

        private bool bLoading = false;
        public void SetSprite(string spriteName,AssetInfo altasAssetInfo)
        {
            if(spriteName == m_AssetInfo.assetPath)
            {
                return;
            }

            if (spriteName == string.Empty)
            {
                RelaseSprite();
                return;
            }
            m_AssetInfo.assetPath = spriteName;
            m_AssetInfo.assetType = AssetType.sprite;
            m_AssetInfo.locationType = AssetLocation.Local;
            m_AssetInfo.RefCachePolicy = null;
            bLoading = true;
            Resource.Load(altasAssetInfo, (ab) =>
            {
                Resource.LoadAssetAtAB(m_AssetInfo, (AssetBundle)ab,(obj)=> {
                    OnSpriteLoadFinished(obj, altasAssetInfo);
                } );
            });
        }

        private void RelaseSprite()
        {
            if(null != this.sprite)
            {
                FreeAltasRef();
                this.sprite = null;
                this.m_AltasAssetInfo = null;
            }
        }

        private void OnSpriteLoadFinished(Object obj,AssetInfo altasAssetInfo)
        {
            FreeAltasRef();
            if (null != obj && obj is Sprite)
            {
                this.sprite = obj as Sprite;
                m_AltasAssetInfo = altasAssetInfo;
            }
            else
            {
                Debug.LogError("Sprite Not Exist");
            }
            
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            FreeAltasRef();
        }

        private void FreeAltasRef()
        {
            if (null != m_AltasAssetInfo)
            {
                Resource.Free(m_AltasAssetInfo);
            }
        }
    }
}
