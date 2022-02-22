using _Scripts.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts {
    public class LevelConfig : MonoBehaviour {

        [SerializeField]
        private Camera secondaryCamera;

        [SerializeField]
        private GameObject enemySpawnerParent;

        [SerializeField]
        private Image[] towerSymbolsHolders;

        [SerializeField]
        private LevelCore levelCore;

        [SerializeField]
        private float memoryLimit = 100f;

        [SerializeField]
        private float memoryRegainPercentage = 0.75f;

        [SerializeField]
        private float usedMemory = 0;

        private void Awake () {
            UiController.Instance.SwitchUiElemActive(UiController.UiElement.HUD);
        }

        internal Camera SecondaryCamera { get { return secondaryCamera; } }

        internal GameObject EnemySpawnerParent { get { return enemySpawnerParent; } }

        internal Image[] TowerSymbolsHolders {
            get { return towerSymbolsHolders; }
        }

        internal LevelCore LvlCore {
            get { return levelCore; }
            set { levelCore = value; }
        }

        internal float MemoryLimit { get { return memoryLimit; } }

        internal float MemoryRegainPercentage {
            get { return memoryRegainPercentage; }
            set { memoryRegainPercentage = value; }
        }

        internal float UsedMemory {
            get { return usedMemory; }
            set { usedMemory = value; }
        }
    }
}