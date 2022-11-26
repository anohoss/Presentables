using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace anoho.Presentables {
    public class CoroutineRunner : MonoBehaviour {
        private static CoroutineRunner s_instance;

        public static CoroutineRunner Instance {
            get {
                if (s_instance == null) {
                    GameObject go = new GameObject("[Transition Coroutine Runner]");
                    s_instance = go.AddComponent<CoroutineRunner>();

                    DontDestroyOnLoad(go);
                }

                return s_instance;
            }
        }

        private void OnDestroy() {
            StopAllCoroutines();
            s_instance = null;
        }
    }
}

