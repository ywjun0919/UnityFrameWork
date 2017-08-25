

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
            if (Input.GetKey(KeyCode.C))
            {
                Loader.Destory("ui/dlgtest.ui");
            }
            else if (Input.GetKey(KeyCode.O))
            {
                Loader.LoadUI("ui/dlgtest.ui",null);
            }
            else if(Input.GetKey(KeyCode.U))
            {
                GameMain.SetSprite("google");
            }
            else if(Input.GetKey(KeyCode.I))
            {
                GameMain.SetSprite("1_cjsen");
            }
        }

    }
}
