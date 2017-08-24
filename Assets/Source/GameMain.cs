
using System.Collections;
using UnityEngine;

namespace Game
{
    class GameMain:MonoBehaviour
    {
        private static GameMain Instance = null; 
        private void Awake()
        {
            Instance = this.GetComponent<GameMain>();
            InputManager.Init();
            Loader.LoadUI("ui/dlgtest.ui");
        }
        internal static Coroutine Coroutine(IEnumerator routine)
        {
            return Instance.StartCoroutine(routine);
        }

    }
}
