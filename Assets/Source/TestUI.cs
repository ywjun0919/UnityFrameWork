using UnityEngine;

namespace Game
{
    class TestUI
    {
        private static TestUI m_Instance = new TestUI();
        
        public static TestUI Instance {
            get { return m_Instance; }
        }

        public GameObject uiObj = null;

        public void SetSprite(string spriteName)
        {
            UnityEngine.UI.UISprite img = uiObj.transform.FindChild("sprite").GetComponent<UnityEngine.UI.UISprite>();
            img.SetSprite(spriteName, Loader.CreateAssetInfo("ui/atlas/common.ui"));
        }

        public void SetTexture(string TextureName)
        {
            UnityEngine.UI.UITexture img = uiObj.transform.FindChild("Texture").GetComponent<UnityEngine.UI.UITexture>();
            img.SetTexture(Loader.CreateAssetInfo("texture/"+TextureName+ ".bundle"));
        }
    }
}
