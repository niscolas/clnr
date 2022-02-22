using _Scripts.Controllers;
using _Scripts.Enemies;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using static _Scripts.Controllers.UiController;

namespace _Scripts.Managers {
    public class GameManager : MonoBehaviour {

        private static GameManager _instance;

        [SerializeField]
        private FirstPersonController fpsController;

        [SerializeField]
        private Player player;

        [SerializeField]
        private Slider coreHealthSlider;

        [SerializeField]
        private Slider memorySlider;

        [SerializeField]
        private Material dissolveMaterial;

        [SerializeField]
        private Shader dissolveShader;

        [SerializeField]
        private TextMeshProUGUI selectedTowerTxt;

        [SerializeField]
        private bool godMode = false;

        [SerializeField]
        private bool highQualityGraphics = true;

        public Scene actualScene { get; set; }

        private AudioManager audioManager;
        private BuildingManager buildingManager;
        private Image[] towerSymbolsHolders;
        private LevelConfig levelConfig;
        private UiController uiController;
        private bool levelHasFinished = false;
        private int totalEnemies;

        public static GameManager Instance { get { return _instance; } }

        public FirstPersonController FPSController { get { return fpsController; } }

        private GameObject EnemySpawnerParent { get { return levelConfig.EnemySpawnerParent; } }

        public LevelCore LvlCore { get { return levelConfig.LvlCore; } }

        public Slider CoreHealthSlider { get { return coreHealthSlider; } }

        public Material DissolveMaterial { get { return dissolveMaterial; } }

        public Shader DissolveShader { get { return dissolveShader; } }

        public TextMeshProUGUI SelectedTowerTxt { get { return selectedTowerTxt; } }

        public bool GodMode {
            get { return godMode; }
            set { godMode = value; }
        }

        public bool HighQualityGraphics {
            get { return highQualityGraphics; }
            set {
                if (highQualityGraphics == value) {
                    return;
                }

                highQualityGraphics = value;

                HighQualityChange?.Invoke(highQualityGraphics);
            }
        }
        public delegate void HighQualityChangeDelegate(bool highQuality);
        public event HighQualityChangeDelegate HighQualityChange;

        public int TotalEnemies {
            get { return totalEnemies; }
            set {
                totalEnemies = value;
                Debug.Log ("GM: " + totalEnemies + " remaining enemies");
            }
        }

        private float UsedMemory {
            get { return levelConfig.UsedMemory; }
            set {
                if (!godMode) {
                    if (value <= 0f) {
                        levelConfig.UsedMemory = 0f;
                    } else if (value >= 100f) {
                        levelConfig.UsedMemory = 100f;
                    } else {
                        levelConfig.UsedMemory = value;
                    }

                    memorySlider.value = (levelConfig.UsedMemory / memorySlider.maxValue) * 100f;
                }
            }
        }

        private float MemoryLimit { get { return levelConfig.MemoryLimit; } }

        private float MemoryRegainPercentage {
            get { return levelConfig.MemoryRegainPercentage; }
            set { levelConfig.MemoryRegainPercentage = value; }
        }

        private void Awake () {
            EnsureSingleton ();

            DontDestroyOnLoad (gameObject);
        }

        private void OnEnable () {
            SceneManager.sceneLoaded -= GameManager.Instance.OnSceneLoaded;
            SceneManager.sceneLoaded += GameManager.Instance.OnSceneLoaded;
            Debug.Log ("GM: Added Delegate to sceneLoaded");
        }

        private void OnDisable () {
            SceneManager.sceneLoaded -= GameManager.Instance.OnSceneLoaded;
        }

        private void Start () {
            buildingManager = BuildingManager.Instance;
            audioManager = AudioManager.Instance;
            uiController = UiController.Instance;
        }

        private void Update () {
            if (levelConfig != null && !levelHasFinished) {
                if (LvlCore.Health <= 0) {
                    levelHasFinished = true;
                    GameOver ();
                } else if (totalEnemies == 0) {
                    levelHasFinished = true;
                    Victory ();
                }
            }
        }

        private void EnsureSingleton () {
            if (_instance == null) {
                _instance = this;
            } else if (_instance != this) {
                Destroy (gameObject);
            }
        }

        /// <summary>
        ///     When a new Level is loaded, the propeties of the game Manager will 
        ///     be updated by the Level Config object on the Scene
        /// </summary>
        public void OnSceneLoaded (Scene scene, LoadSceneMode mode) {
            Debug.Log ("GM: New scene loaded");

            actualScene = scene;
            Debug.Log ("GM: -> Actual Scene: " + actualScene.name);

            // Gets the Level Config on the Scene
            levelConfig = FindObjectOfType<LevelConfig> ();

            if (levelConfig != null) {
                Debug.Log ("GM: Level Config was found");
                DoLevelStartConfig (levelConfig);
            }
        }

        private void DoLevelStartConfig (LevelConfig levelConfig) {
            Debug.Log ("GM: Start Level config");

            levelHasFinished = false;

            totalEnemies = GetTotalEnemies ();
            Debug.Log ("GM: -> totalEnemies: " + totalEnemies);

            // SetCardImages ();

            player = Player.Instance;
            if (player != null) {
                fpsController = player.GetComponent<FirstPersonController> ();
            }

            memorySlider.maxValue = 100f;
            memorySlider.value = UsedMemory;
        }

        private int GetTotalEnemies () {
            int totalEnemies = 0;
            for (int i = 0; i < EnemySpawnerParent.transform.childCount; i++) {
                EnemySpawner actualSpawner = EnemySpawnerParent.transform.GetChild (i).GetComponent<EnemySpawner> ();
                totalEnemies += actualSpawner.GetTotalEnemyCount ();
            }

            return totalEnemies;
        }

        private void SetCardImages () {
            /*
            for (int i = 0; i < towerSymbolsHolders.Length; i++) {
                towerSymbolsHolders[i].sprite = buildingManager.AvaiableTowers[i].TowerSymbol;
            }
            */
        }

        /// <summary>
        ///     Checks if there's enough Avaiable Memory
        /// </summary>
        /// <param name="memoryCost"></param>
        /// <returns> Wheter there's enough Free Memory to put 'memoryCost' on it </returns>
        private bool HasEnoughAvaiableMemory (float memoryCost) {
            return memoryCost <= MemoryLimit - UsedMemory;
        }

        /// <summary>
        ///     Try to Free or Occupy memory.
        ///     If 'memoryCost' is positive, try to occupy that requested ammount of memory,
        ///         if there is not enough free Memory
        ///         otherwise occupy the specified ammount
        ///     If is negative, just free the requested ammount
        /// </summary>
        /// <param name="memoryCost"> The ammount of Memory that should be freed or occupied </param>
        /// <returns> Whether the operation was successful or not </returns>
        public bool TryToChangeMemoryUse (float memoryCost, bool towerMemoryRegain = true) {
            if (!GodMode) { 
                if (memoryCost <= 0) {
                    if (towerMemoryRegain) {
                        UsedMemory += memoryCost * MemoryRegainPercentage;
                    } else {
                        UsedMemory += memoryCost;
                    }

                    return true;
                } else if (memoryCost > 0) {
                    if (HasEnoughAvaiableMemory (memoryCost)) {
                        UsedMemory += memoryCost;
                        return true;
                    }
                }
            } else {
                return true;
            }
            return false;
        }

        public void GameOver () {
            Debug.Log ("Game Over");

            uiController.SwitchPausedGame ();
            uiController.SwitchUiElemActive (UiElement.HUD);
            uiController.SwitchUiElemActive (UiElement.DEFEAT);
        }

        public void Victory () {
            Debug.Log ("Victory");

            uiController.SwitchPausedGame ();
            uiController.SwitchUiElemActive (UiElement.HUD);
            uiController.SwitchUiElemActive (UiElement.VICTORY);
        }

        public void LoadNextLevel () {
            if (actualScene.name == "Level1") {
                SceneController.LoadSceneByName ("Level2");
            } else if (actualScene.name == "Level2") {
                SceneController.LoadSceneByName ("Level3");
            } else {
                SceneController.LoadSceneByName ("MainMenu");
            }
            uiController.SwitchUiElemActive (UiElement.VICTORY);
        }
    }
}