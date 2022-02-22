using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.Enemies {
    public class Enemy : MonoBehaviour {

        [SerializeField]
        private float damage = 1f;

        [SerializeField]
        private float health = 10f;

        [SerializeField]
        private float memoryBackAmmount = 5f;

        [SerializeField]
        private float speed = 5f;

        private GameManager gameManager;
        private Transform waypointsParentGO;
        private Transform targetPathNode;
        private bool alreadyDied = false;
        private int waypointIndex = 0;

        internal float Damage {
            get { return damage; }
            set { damage = value; }
        }

        internal float Health {
            get { return health; }
            set { health = value; }
        }

        internal float Speed {
            get { return speed; }
            set { speed = value; }
        }

        public Transform WaypointsParentGO { set { waypointsParentGO = value; } }

        private void Start () {
            gameManager = GameManager.Instance;

            if (waypointsParentGO == null) {
                Debug.Log ("The Waypoint Parent will be found automatically");

                GameObject[] possiblePaths = GameObject.FindGameObjectsWithTag ("Path");

                if (possiblePaths.Length == 1) {
                    waypointsParentGO = possiblePaths[0].transform;
                }
            } else {
                Debug.Log ("The Waypoint Parent was already set: " + waypointsParentGO);
            }

            GetNextPathNode ();
        }

        private void Update () {
            if (targetPathNode == null) {
                // Last Node reached, the Enemy will be destroyed
                PathEndReached ();
                return;
            } else {
                MoveTowardsNode ();
            }
        }

        private void GetNextPathNode () {
            if (waypointIndex < waypointsParentGO.transform.childCount) {
                targetPathNode = waypointsParentGO.transform.GetChild (waypointIndex);
                waypointIndex++;
            } else {
                targetPathNode = null;
                PathEndReached ();
            }
        }

        private void MoveTowardsNode () {
            Vector3 dir = targetPathNode.position - this.transform.localPosition;

            float distThisFrame = speed * Time.deltaTime;

            if (dir.magnitude <= distThisFrame) {
                // The Target Node was reached  
                GetNextPathNode ();
            } else {
                transform.Translate (dir.normalized * distThisFrame, Space.World);
                Quaternion targetRotation = Quaternion.LookRotation (dir);
                this.transform.rotation = Quaternion.Lerp (this.transform.rotation, targetRotation, Time.deltaTime * 5);
            }
        }

        private void PathEndReached () {
            Die ();
        }

        public void TakeDamage (float damage) {
            health -= damage;

            Die ();
        }

        private void Die () {
            if (health <= 0 && !alreadyDied) {
                alreadyDied = true;

                SpawnEffect spawnEffect = gameObject.GetComponent<SpawnEffect> ();
                DoDissolveEffect (spawnEffect);

                Destroy (gameObject, spawnEffect.spawnEffectTime);
                gameManager.TotalEnemies--;
                gameManager.TryToChangeMemoryUse (-memoryBackAmmount, false);
            }
        }

        private void DoDissolveEffect (SpawnEffect spawnEffect) {
            Renderer renderer = gameObject.GetComponent<Renderer> ();
            renderer.material = gameManager.DissolveMaterial;
            renderer.material.shader = gameManager.DissolveShader;

            spawnEffect.enabled = true;
        }
    }
}