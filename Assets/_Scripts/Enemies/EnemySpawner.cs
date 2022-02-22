using System;
using UnityEngine;

/*
    Original Author: Quill18 - quill18@quill18.com
    Modified by: Nícolas Catarina Parreiras - nic.cp@hotmail.com
    Date: 03/11/2019
*/
namespace _Scripts.Enemies {
    public class EnemySpawner : MonoBehaviour {

        [Serializable]
        protected class WaveComponent {
            [SerializeField]
            private Enemy enemyPrefab;

            [SerializeField]
            private Transform waypointsParentGO;

            [SerializeField]
            private int numOfEnemies;

            [NonSerialized]
            private int spawned = 0;

            internal Enemy EnemyPrefab {
                get { return enemyPrefab; }
                set { enemyPrefab = value; }
            }

            internal Transform WaypointsParentGO {
                get { return waypointsParentGO; }
                set { waypointsParentGO = value; }
            }

            internal int NumOfEnemies {
                get { return numOfEnemies; }
                set { numOfEnemies = value; }
            }

            internal int Spawned {
                get { return spawned; }
                set { spawned = value; }
            }
        }

        [Header ("Each type and quantity of Enemy that will be spawned")]
        [SerializeField]
        private WaveComponent[] waveComponents;

        [Header ("Time in Seconds, between Each Enemy Spawn")]
        [SerializeField]
        private float timeBetweenEnemies;

        [Header ("Time in Seconds, before the first Enemy Spawn")]
        [SerializeField]
        private float timeUntilSpawnStart;

        private System.Random rand;
        private SpawnController spawnController;

        private void Awake () {
            Debug.Log ("Awaking: " + this);

            rand = new System.Random ();
        }

        private void Start () {
            spawnController = SpawnController.Instance;
            if (spawnController.RandomPaths) {
                Transform[] possiblePaths = spawnController.PossiblePaths;
                foreach (WaveComponent wc in waveComponents) {
                    int pathPos = rand.Next (0, possiblePaths.Length);
                    Transform chosenPath = possiblePaths[pathPos];

                    Debug.Log("The chosen path for " + wc.EnemyPrefab + " is " + chosenPath);

                    wc.WaypointsParentGO = chosenPath;
                }
            }
        }

        private void Update () {
            timeUntilSpawnStart -= Time.deltaTime;

            if (timeUntilSpawnStart < 0) {
                timeUntilSpawnStart = timeBetweenEnemies;

                bool didSpawn = false;

                // Go through the wave comps until we find something to spawn;
                foreach (WaveComponent wc in waveComponents) {
                    if (wc.Spawned < wc.NumOfEnemies) {
                        SpawnEnemy (wc);
                        didSpawn = true;
                        break;
                    }
                }

                if (didSpawn == false) {
                    OnWaveFinish ();
                }
            }
        }

        public void StartWaveIgnoringStartTime () {
            timeUntilSpawnStart = 0;
        }

        private void SpawnEnemy (WaveComponent wc) {
            wc.Spawned++;

            Enemy spawnedEnemy = Instantiate (wc.EnemyPrefab, wc.WaypointsParentGO.GetChild (0).position, wc.WaypointsParentGO.GetChild (0).rotation);

            if (wc.WaypointsParentGO != null) {
                Debug.Log ("Setting the path of the Enemy");
                spawnedEnemy.WaypointsParentGO = wc.WaypointsParentGO;
            }
        }

        private void OnWaveFinish () {
            // There are waves remaining
            if (transform.parent.childCount > 1) {
                Debug.Log ("Initiating next wave");
                transform.parent.GetChild (1).gameObject.SetActive (true);
            }
            // Last Wave
            else {

            }

            Destroy (gameObject);
        }

        public int GetTotalEnemyCount () {
            int totalEnemies = 0;
            foreach (var wC in waveComponents) {
                totalEnemies += wC.NumOfEnemies;
            }

            return totalEnemies;
        }
    }
}