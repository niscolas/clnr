using System.Collections;
using _Scripts.Utils;
using _Scripts.Utils.UI.Image;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace _Scripts.Controllers.Screens {
    public class OpeningScreenController : MonoBehaviour {

        public Image paramBackground;
        public Image[] paramImgs;
        public ImageFader.FadeSpeed paramFadeSpeed;
        public VideoPlayer[] paramVps;
        public string paramSceneToLoadNext = "MainMenu";

        private CoroutineUtils coroutineUtils;
        private IEnumerator[] coroutines;
        private ImageFader imageFader;
        private VideoController videoController;

        private void Awake () {
            // Get Instances of the Helpers
            coroutineUtils = CoroutineUtils.Instance;
            imageFader = ImageFader.Instance;
            videoController = VideoController.Instance;

            // Set the Alpha of every Image to 0, so they won't overlap
            // and the animation will run normally
            foreach (Image img in paramImgs) {
                Color tempColor = img.color;
                tempColor.a = 0f;
                img.color = tempColor;
            }

            coroutines = new IEnumerator[] {
                PlayImagesAnim (), 
                PlayVideosAnim ()
                };
        }

        private IEnumerator Start () {
            yield return StartCoroutine(coroutineUtils.StartCoroutines(coroutines));
            SceneController.LoadSceneByName (paramSceneToLoadNext);
        }

        private IEnumerator PlayImagesAnim () {
            paramBackground.gameObject.SetActive (true);
            foreach (Image img in paramImgs) {
                IEnumerator cr = imageFader.CycleFade (img, paramFadeSpeed);
                yield return StartCoroutine (cr);
            }
            paramBackground.gameObject.SetActive (false);
        }

        private IEnumerator PlayVideosAnim () {
            IEnumerator videoSequencePlayerCr = videoController.PlayVideosInSequence (paramVps);
            yield return StartCoroutine (videoSequencePlayerCr);
        }

        private void Update () {
            if (Input.GetKeyDown ("space")) {
                // StartCoroutine(coroutineUtils.StopCoroutines(coroutines));
                SceneController.LoadSceneByName (paramSceneToLoadNext);
            }
        }
    }
}