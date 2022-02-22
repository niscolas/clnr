using _Scripts.Managers;
using UnityEngine;

namespace _Scripts {
    public class HighQualityObject : MonoBehaviour {

        private GameManager gameManager;
        private bool highQualityGraphicsLastState;

        private void Start() {
            gameManager = GameManager.Instance;

            gameManager.HighQualityChange += OnQualityChangeHandler;

            highQualityGraphicsLastState = gameManager.HighQualityGraphics;
        }

        private void OnQualityChangeHandler (bool highQualityEnable) {
            gameObject.SetActive(highQualityEnable);
        }

        private void Update () {
        
        }
    }
}