using System.Collections;
using UnityEngine;

namespace _Scripts.Utils {
    public class CoroutineUtils : MonoBehaviour {

        private static CoroutineUtils _instance = null;
        private CoroutineUtils () { }

        public static CoroutineUtils Instance {
            get {
                if (_instance == null) {
                    GameObject container = new GameObject ("CoroutineUtils");
                    _instance = container.AddComponent<CoroutineUtils> ();
                }
                return _instance;
            }
        }

        public IEnumerator StartCoroutines (IEnumerator[] coroutines) {
            foreach (IEnumerator coroutine in coroutines) {
                yield return StartCoroutine (coroutine);
            }
        }

        public void StopCoroutines (IEnumerator[] coroutines) {
            foreach (IEnumerator coroutine in coroutines) {
                StopCoroutine (coroutine);
            }
        }
    }
}