using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

namespace _Scripts.Controllers {
    public class VideoController : MonoBehaviour {
        private static VideoController _instance = null;

        private VideoPlayer[] vpsToPlayInSequence;

        private bool lastVideoFinished = false;

        public static VideoController Instance {
            get {
                if (_instance == null) {
                    GameObject container = new GameObject ("VideoController");
                    _instance = container.AddComponent<VideoController> ();
                }
                return _instance;
            }
        }

        public IEnumerator PlayVideosInSequence (VideoPlayer[] vpsToPlayInSequence) {
            this.vpsToPlayInSequence = vpsToPlayInSequence;

            Debug.Log ("VideoPlayer Array Length: " + vpsToPlayInSequence.Length);

            vpsToPlayInSequence[0].Play ();

            for (int i = 0; i < vpsToPlayInSequence.Length; i++) {
                Debug.Log ("Actual Video Player Index: " + i);

                vpsToPlayInSequence[i].loopPointReached += EndReached;
            }
            yield return StartCoroutine(WaitUntilLastVideoFinish());
        }

        private void EndReached (VideoPlayer vp) {
            int indexOfVp = Array.IndexOf (vpsToPlayInSequence, vp);

            Debug.Log ("Actual Index: " + indexOfVp);
            if (indexOfVp == vpsToPlayInSequence.Length - 1) {
                lastVideoFinished = true;    
            } else {
                vpsToPlayInSequence[indexOfVp + 1].Play ();
            }
        }

        private IEnumerator WaitUntilLastVideoFinish () {
            while (!lastVideoFinished) {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}