using _Scripts._Enums;
using _Scripts.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Controllers {
    public class SceneController : MonoBehaviour {
        public static void LoadSceneByGameScene (GameScene sceneName) {
            TimeManager.Instance.ResumeGame ();
            SceneManager.LoadScene ((int) sceneName);
        }

        internal static void LoadSceneByName (string sceneName) {
            if (TimeManager.Instance != null) {
                if (TimeManager.Instance.isPaused) {
                    UiController.Instance.SwitchPausedGame ();
                    UiController.Instance.LockOrUnlockCursor();
                }
            }

            if (GameManager.Instance != null) {
                if (sceneName == "MainMenu") {
                    Destroy (GameManager.Instance.gameObject);
                }
            }

            SceneManager.LoadScene (sceneName);
        }

        public void LoadSomeScene (string sceneName) {
            SceneController.LoadSceneByName (sceneName);
        }

        public void ExitGame () {
            Application.Quit ();
        }
    }
}