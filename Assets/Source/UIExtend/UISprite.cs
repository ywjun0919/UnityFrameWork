using Game;
using System;


namespace UnityEngine.UI
{
    class UISprite : Image
    {
        private AssetInfo m_AssetInfo = new AssetInfo();
        private AssetInfo m_AltasAssetInfo = null;

        public void SetSprite(string spriteName,AssetInfo altasAssetInfo)
        {
            if(spriteName == m_AssetInfo.assetPath)
            {
                return;
            }
            if(spriteName == string.Empty)
            {
                FreeAltasRef();
                this.sprite = null;
                return;
            }
            m_AssetInfo.assetPath = spriteName;
            m_AssetInfo.assetType = AssetType.sprite;
            m_AssetInfo.locationType = AssetLocation.Local;
            m_AssetInfo.RefCachePolicy = null;

            Resource.Load(altasAssetInfo, (ab) =>
            {
                Resource.LoadAssetAtAB(m_AssetInfo, (AssetBundle)ab,(obj)=> {
                    OnSpriteLoadFinished(obj, altasAssetInfo);
                } );
            });
        }

        private void OnSpriteLoadFinished(Object obj,AssetInfo altasAssetInfo)
        {
           
            if(null != obj && obj is Sprite)
            {
                this.sprite = obj as Sprite;
               
                if ( null != m_AltasAssetInfo && m_AltasAssetInfo.assetPath != altasAssetInfo.assetPath)
                {
                    FreeAltasRef();
                }
                m_AltasAssetInfo = altasAssetInfo;
            }
            else
            {
                Resource.Free(altasAssetInfo);
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
