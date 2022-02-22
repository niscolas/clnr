using UnityEngine;

namespace _Scripts.Managers {
    public class TimeManager : MonoBehaviour {
        private static TimeManager _instance;

        public bool isPaused = false;

        public static TimeManager Instance { get { return _instance; } }

        private void Awake () {
            EnsureSingleton();
        }

        private void EnsureSingleton () {
            if (_instance == null) {
                _instance = this;
            } else if (_instance != this) {
                Destroy (gameObject);
            }
        }

        public void PauseGame () {
            UpdateTimeScale (0f);
        }

        public void ResumeGame () {
            UpdateTimeScale (1f);
        }

        public void UpdateTimeScale (float timeScaleMultiplier) {
            Time.timeScale = timeScaleMultiplier;

            if (timeScaleMultiplier == 0) {
                isPaused = true;
            } else {
                isPaused = false;
            }
        }
    }
}