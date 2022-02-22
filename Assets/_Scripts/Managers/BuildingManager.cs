using _Scripts.Towers;
using TMPro;
using UnityEngine;

namespace _Scripts.Managers {
    public class BuildingManager : MonoBehaviour {

        public static BuildingManager _instance = null;

        private GameManager gameManager;

        [SerializeField]
        private Material availableMaterial;

        [SerializeField]
        private Material unavailableMaterial;

        [SerializeField]
        private Tower[] avaiableTowers;

        [SerializeField]
        private int selectedTowerIndex = 0;

        private TextMeshProUGUI selectedTowerTxt;

        public static BuildingManager Instance { get { return _instance; } }

        public Material AvailableMaterial { get { return availableMaterial; } }

        public Material UnavailableMaterial { get { return unavailableMaterial; } }

        public Tower[] AvaiableTowers { get { return avaiableTowers; } }

        public int SelectedTowerIndex {
            get { return selectedTowerIndex; }
            set {
                if (selectedTowerIndex == value) {
                    return;
                }

                selectedTowerIndex = value;

                if (OnSelectedTowerChange != null) {
                    OnSelectedTowerChange(selectedTowerIndex);
                }
            }
        }
        public delegate void OnSelectedTowerChangeDelegate (int selectedTowerIndex);
        public event OnSelectedTowerChangeDelegate OnSelectedTowerChange;

        public Tower SelectedTower { get { return avaiableTowers[selectedTowerIndex]; } }

        private void Awake () {
            Debug.Log("BM: Awaking Building Manager");

            EnsureSingleton ();
            if (OnBMInstanceCreated != null) {
                OnBMInstanceCreated();
            }

            DontDestroyOnLoad (gameObject);
        }
        public delegate void OnBMInstanceCreatedDelegate();
        public static OnBMInstanceCreatedDelegate OnBMInstanceCreated;

        private void Start () {
            gameManager = GameManager.Instance;

            selectedTowerTxt = gameManager.SelectedTowerTxt;
            SelectedTowerIndex = selectedTowerIndex;
        }

        private void EnsureSingleton () {
            if (_instance == null) {
                _instance = this;
            } else if (_instance != this) {
                Destroy (gameObject);
            }
        }
    }
}