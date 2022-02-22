using _Scripts.Enemies;
using UnityEngine;

namespace _Scripts.Towers.Bullets {
    public class Bullet : MonoBehaviour {

        [SerializeField]
        private ParticleSystem bulletParticle;

        [SerializeField]
        private ParticleSystem explosionParticle;

        [SerializeField]
        private float damage = 1f;

        [SerializeField]
        private float radius = 1f;

        [SerializeField]
        private float speed = 15f;

        private bool alreadyHitTarget = false;
        private Transform targetTfm;

        public Transform TargetTfm {
            get { return targetTfm; }
            set { targetTfm = value; }
        }

        public float Damage {
            get { return damage; }
            set { damage = value; }
        }

        public float Radius {
            get { return radius; }
            set { radius = value; }
        }

        public float Speed {
            get { return speed; }
            set { speed = value; }
        }

        private void Update () {
            if (TargetTfm == null) {
                Explode ();
                return;
            }
            MoveTowardsTarget ();
        }

        private void MoveTowardsTarget () {
            Vector3 dir = TargetTfm.position - this.transform.localPosition;

            float distThisFrame = speed * Time.deltaTime;

            transform.Translate (dir.normalized * distThisFrame, Space.World);
            Quaternion targetRotation = Quaternion.LookRotation (dir);
            this.transform.rotation = Quaternion.Lerp (this.transform.rotation, targetRotation, Time.deltaTime * 5);
        }

        private void OnTriggerEnter (Collider other) {
            if (other.gameObject.TryGetComponent (out Enemy enemyCollided)) {
                DoBulletHit (enemyCollided);
            }
        }

        private void DoBulletHit (Enemy collidedEnemy) {
            if (radius == 0) {
                collidedEnemy.TakeDamage (damage);
            } else {
                Collider[] colls = Physics.OverlapSphere (transform.position, radius);

                foreach (Collider c in colls) {
                    Enemy e = c.GetComponent<Enemy> ();
                    if (e != null) {
                        e.GetComponent<Enemy> ().TakeDamage (damage);
                    }
                }
            }

            Explode ();
        }

        private void Explode () {
            if (!alreadyHitTarget) {
                alreadyHitTarget = true;

                if (bulletParticle != null) {
                    Destroy (bulletParticle.gameObject);
                }

                if (explosionParticle != null) {

                    DoExplosionEffect ();
                } else {
                    Destroy (gameObject);
                }
            }
        }

        private void DoExplosionEffect () {
            explosionParticle.Play ();

            float totalTimeBeforeDestroy = explosionParticle.main.duration + explosionParticle.main.startLifetimeMultiplier;
            Destroy (gameObject, totalTimeBeforeDestroy);
        }
    }
}