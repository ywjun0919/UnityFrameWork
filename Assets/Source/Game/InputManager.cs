

using UnityEngine;

namespace Game
{
    public class InputManager : MonoBehaviour
    {
        static InputManager m_Instance = null;
        public static void Init()
        {
            var obj = Camera.main.gameObject;
            obj.AddComponent<InputManager>();
            m_Instance = obj.AddComponent<InputManager>();
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.D))
            {
                Loader.Destory("ui/dlgtest.ui");
            }
            else if (Input.GetKey(KeyCode.O))
            {
                Loader.LoadUI("ui/dlgtest.ui", (obj)=> { TestUI.Instance.uiObj = obj; });
            }
            else if (Input.GetKey(KeyCode.U))
            {
                TestUI.Instance.SetSprite("google");
            }
            else if (Input.GetKey(KeyCode.E))
            {
                TestUI.Instance.SetSprite("");
            }
            else if (Input.GetKey(KeyCode.I))
            {
                TestUI.Instance.SetSprite("1_cjsen");
            }
            else if (Input.GetKey(KeyCode.T))
            {
                TestUI.Instance.SetTexture("1");
            }
            else if(Input.GetKey(KeyCode.C))
            {
                Cache.GetOrCreateCache("T");
            }
        }

    }
}
