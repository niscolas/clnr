using UnityEngine;

namespace _Scripts.Enemies {
    public class SpawnController : MonoBehaviour {

        private static SpawnController _instance;

        [SerializeField]
        private Enemy[] possibleEnemies = null;

        [SerializeField]
        private Transform[] possiblePaths = null;

        [SerializeField]
        private bool randomPaths;

        [SerializeField]
        private bool randomizeEverything;

        public static SpawnController Instance { get { return _instance; } }

        public bool RandomPaths { get { return randomPaths; } }

        public Transform[] PossiblePaths { get { return possiblePaths; } }

        private void Awake () {
            EnsureSingleton ();

            if (possiblePaths == null || possiblePaths.Length == 0) {
                FindPathsAutomatically();
            }
        }

        private void FindPathsAutomatically () {
            Debug.Log("SpawnController: The paths will be found automatically");

            GameObject[] possiblePathsGO = GameObject.FindGameObjectsWithTag("Path");
            possiblePaths = new Transform[possiblePathsGO.Length];

            for (int i = 0; i < possiblePathsGO.Length; i++)
            {
                possiblePaths[i] = possiblePathsGO[i].transform;
            }
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