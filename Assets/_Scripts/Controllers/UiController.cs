using _Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Controllers {
    public class UiController : MonoBehaviour {

        private static UiController _instance;

        public static UiController Instance { get { return _instance; } }

        public enum UiElement {
            EXIT,
            DEFEAT,
            HUD,
            PAUSE,
            SETTINGS,
            VICTORY,
        }

        [SerializeField]
        private GameObject defeatScreen;

        [SerializeField]
        private GameObject hud;

        [SerializeField]
        private GameObject pauseMenu;

        [SerializeField]
        private GameObject settingsMenu;

        [SerializeField]
        private GameObject victoryScreen;

        [SerializeField]
        private Transform towersCardsParent;

        private Image[] towersCardsImgs;
        private BuildingManager buildingManager;
        private GameManager gameManager;
        private TimeManager timeManager;

        private void Awake () {
            EnsureSingleton ();

            towersCardsImgs = new Image[towersCardsParent.childCount];
            for (int i = 0; i < towersCardsImgs.Length; i++) {
                towersCardsImgs[i] = towersCardsParent.GetChild(i).GetComponent<Image>();
            }
        }

        private void Start () {
            timeManager = TimeManager.Instance;
            gameManager = GameManager.Instance;

            BuildingManager.OnBMInstanceCreated -= OnBuildingManagerInstanceCreatedHandler;
            BuildingManager.OnBMInstanceCreated += OnBuildingManagerInstanceCreatedHandler;
        }

        private void OnDestroy() {
            BuildingManager.OnBMInstanceCreated -= OnBuildingManagerInstanceCreatedHandler;
            buildingManager.OnSelectedTowerChange -= SelectedTowerChangeHandler;
        }

        private void OnBuildingManagerInstanceCreatedHandler() {
            buildingManager = BuildingManager.Instance;
            buildingManager.OnSelectedTowerChange -= SelectedTowerChangeHandler;
            buildingManager.OnSelectedTowerChange += SelectedTowerChangeHandler;
        }

        private void EnsureSingleton () {
            if (_instance == null) {
                _instance = this;
            } else if (_instance != this) {
                Destroy (gameObject);
            }
        }

        public void SwitchUiElemActive (UiElement uiElem) {
            GameObject targetMenu = null;

            switch (uiElem) {
                // case Menu.EXIT:
                //     break;

                case UiElement.DEFEAT:
                    targetMenu = defeatScreen;
                    break;

                case UiElement.HUD:
                    targetMenu = hud;
                    break;

                case UiElement.PAUSE:
                    targetMenu = pauseMenu;
                    break;

                case UiElement.SETTINGS:
                    targetMenu = settingsMenu;
                    break;

                case UiElement.VICTORY:
                    targetMenu = victoryScreen;
                    break;
            }

            if (targetMenu != null) {
                if (targetMenu.activeSelf) {
                    targetMenu.SetActive (false);
                } else {
                    targetMenu.SetActive (true);
                }
            }
        }

        public void SwitchPausedGame () {
            Debug.Log ("UiC: Switching the game timeScale");

            LockOrUnlockCursor (!timeManager.isPaused);

            if (timeManager.isPaused) {
                timeManager.ResumeGame ();
            } else {
                timeManager.PauseGame ();
            }
        }

        public void LockOrUnlockCursor (bool unlockCursor = true) {
            string debugMsg = (unlockCursor ? "Unlocking the cursor" : "Locking the cursor");
            Debug.Log ("UiC: " + debugMsg);

            Cursor.visible = unlockCursor;

            if (gameManager.FPSController != null) {
                gameManager.FPSController.MouseLook.lockCursor = !unlockCursor;
                gameManager.FPSController.enabled = !unlockCursor;
            }

            if (unlockCursor) {
                Cursor.lockState = CursorLockMode.None;
            } else {
                Cursor.lockState = CursorLockMode.Confined;
            }
        }

        private void SelectedTowerChangeHandler (int selectedTowerIndex) {
            Debug.Log("UiC: SelectedTowerChangeHandler called");

            for (int i = 0; i < towersCardsImgs.Length; i++) {
                Debug.Log("UiC: Iterating through towersCardsImgs");

                Color tempColor = towersCardsImgs[i].color;
                Debug.Log("UiC: -> Old Color Alpha: " + tempColor.a);
                if (i != selectedTowerIndex) {
                    tempColor.a = 0;
                } else {
                    tempColor.a = 255f;
                }
                Debug.Log("UiC: -> New Color Alpha: " + tempColor.a);
                towersCardsImgs[i].color = tempColor;
            }
        }
    }
}