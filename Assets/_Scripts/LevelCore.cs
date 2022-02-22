using _Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts {
    public class LevelCore : MonoBehaviour {

        [SerializeField]
        private float health = 100f;

        private Slider healthSlider;

        public float Health {
            get { return health; }
            set {
                health = value;
                healthSlider.value = (health / healthSlider.maxValue) * 100f;
            }
        }

        private void Start () {
            healthSlider = GameManager.Instance.CoreHealthSlider;

            healthSlider.maxValue = 100f;
            healthSlider.value = 100f;
        }

        public void TakeDamage (float damage) {
            Debug.Log ("LevelCore - healthSlider.value = " + healthSlider.value);
            Debug.Log ("LevelCore - damage = " + damage);

            Debug.Log ("LevelCore - Health = " + health);

            Health -= damage;

            Debug.Log ("LevelCore - Health = " + health);
        }
    }
}