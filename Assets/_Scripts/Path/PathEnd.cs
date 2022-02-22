using _Scripts.Enemies;
using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.Path {
    public class PathEnd : MonoBehaviour {

        private LevelCore levelCore;

        private void Start () {
            levelCore = GameManager.Instance.LvlCore;
        }

        private void OnTriggerEnter (Collider other) {
            if (other.gameObject.TryGetComponent (typeof (Enemy), out Component enemyComponent)) {
                Enemy enemy = enemyComponent.GetComponent<Enemy> ();
                levelCore.TakeDamage (enemy.Damage);

                // If the collider is an enemy, kill it
                enemy.TakeDamage (enemy.Health);
            }
        }
    }
}