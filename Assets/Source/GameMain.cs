
using System.Collections;
using UnityEngine;

namespace Game
{
    class GameMain:MonoBehaviour
    {
        private static GameObject test = null;
        private static GameMain Instance = null; 
        private void Awake()
        {
            Instance = this.GetComponent<GameMain>();
            InputManager.Init();
            Loader.LoadUI("ui/dlgtest.ui",(obj)=> {
                test = obj;
              
               
            });
        
        }
        internal static Coroutine Coroutine(IEnumerator routine)
        {
            return Instance.StartCoroutine(routine);
        }

        /// Test
        /// 
        public static void SetSprite(string spriteName)
        {
            UnityEngine.UI.UISprite img = test.transform.FindChild("sprite").GetComponent<UnityEngine.UI.UISprite>();
            img.SetSprite(spriteName, Loader.CreateAssetInfo("ui/atlas/common.ui"));
        }
    }
}
