using _Scripts.Controllers;
using _Scripts.Managers;
using UnityEngine;

namespace _Scripts {
    public class Player : MonoBehaviour {

        public static Player _instance = null;

        private const int firstAlphaKeyCodeNum = 49;

        private BuildingManager buildingManager;
        private GameManager gameManager;
        private UiController uiController;

        // 1 -> 49
        // 2 -> 50
        // 3 -> 51
        // ...
        private KeyCode[] possibleKeys = {
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
        };

        public static Player Instance { get { return _instance; } }

        private void Awake () {
            EnsureSingleton ();
        }

        private void Start () {
            buildingManager = BuildingManager.Instance;
            gameManager = GameManager.Instance;
            uiController = UiController.Instance;
        }

        private void Update () {
            WatchKeys ();
        }

        private void EnsureSingleton () {
            if (_instance == null) {
                _instance = this;
            } else if (_instance != this) {
                Destroy (gameObject);
            }
        }

        private void WatchKeys () {
            foreach (KeyCode kc in possibleKeys) {
                if (Input.GetKeyDown (kc)) {
                    buildingManager.SelectedTowerIndex = ((int) kc) - firstAlphaKeyCodeNum;
                }
            }

            if (Input.GetKeyDown (KeyCode.Alpha0)) {
                if (Time.timeScale == 20) {
                    Time.timeScale = 1;
                } else {
                    Time.timeScale = 20;
                }
            } 

            if (Input.GetKeyDown (KeyCode.P)) {
                uiController.SwitchPausedGame ();
                uiController.SwitchUiElemActive(UiController.UiElement.PAUSE);
            }
        }
    }
}