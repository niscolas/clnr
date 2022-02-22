using _Scripts.Enemies;
using _Scripts.Towers.Bullets;
using UnityEngine;

namespace _Scripts.Towers {
    public class Tower : MonoBehaviour {

        [SerializeField]
        private Bullet bulletPfb;

        [SerializeField]
        private Sprite towerSymbol;

        [SerializeField]
        private Tower nextLevelPrefab;

        [SerializeField]
        private Transform[] shootPoints;

        [SerializeField]
        private float damage = 1f;

        [SerializeField]
        private float bulletRadius = 0;

        [SerializeField]
        private float fireCooldown = 1f;

        [SerializeField]
        private float range = 10f;

        [SerializeField]
        private int memoryCost = 10;

        private Enemy[] enemies;
        private Enemy nearestEnemy;
        private Vector3 dir;
        float fireCooldownLeft = 0;

        public Sprite TowerSymbol { get { return towerSymbol; } }

        public Tower NextLevelPrefab { get { return nextLevelPrefab; } }

        public int MemoryCost { get { return memoryCost; } }

        private void Awake () {
            fireCooldownLeft = fireCooldown;
        }

        void Update () {
            enemies = GetEnemiesInRange (range);
            nearestEnemy = GetNearestEnemy ();

            if (nearestEnemy == null) {
                Debug.Log ("No enemy found");
                return;
            }

            dir = FollowEnemyPosition ();

            fireCooldownLeft -= Time.deltaTime;
            if (fireCooldownLeft <= 0 && dir.magnitude <= range) {
                fireCooldownLeft = fireCooldown;

                foreach (Transform shootPoint in shootPoints) {
                    ShootAt (shootPoint, nearestEnemy);
                }
            }
        }

        private Vector3 FollowEnemyPosition () {
            dir = nearestEnemy.transform.position - this.transform.position;
            Quaternion lookRot = Quaternion.LookRotation (dir);
            this.transform.rotation = Quaternion.Euler (0, lookRot.eulerAngles.y, 0);

            return dir;
        }

        private Enemy[] GetEnemiesInRange (float range) {
            // TODO: Improve the way it get the enemies, limiting to only the enemies in range
            enemies = GameObject.FindObjectsOfType<Enemy> ();
            return enemies;
        }

        private Enemy GetNearestEnemy () {
            nearestEnemy = null;
            float distNearest = float.MaxValue;

            foreach (Enemy e in enemies) {
                float distToEnemy = Vector3.Distance (this.transform.position, e.transform.position);

                if (nearestEnemy == null || distToEnemy < distNearest) {
                    nearestEnemy = e;
                    distNearest = distToEnemy;
                }
            }
            return nearestEnemy;
        }

        void ShootAt (Transform shootPoint, Enemy e) {
            Bullet bullet = null;
            bullet = Instantiate (bulletPfb, shootPoint.position, shootPoint.rotation);

            bullet.TargetTfm = e.transform;
            bullet.Damage = damage;
            bullet.Radius = bulletRadius;
        }

        private void OnDrawGizmosSelected () {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere (this.transform.position, range);
        }
    }
}