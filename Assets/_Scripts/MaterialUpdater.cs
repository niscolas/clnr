using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts {
    public class MaterialUpdater : MonoBehaviour {

        [Header ("The original material, that is being used")]
        [SerializeField]
        private Material originalMaterial;

        [SerializeField]
        private Material materialToReplaceWith;

        private void Awake () {
            Debug.Log (this + ":\n Awaking the Material Updater");

            string actualSceneName = SceneManager.GetActiveScene ().name;

            Debug.Log (this + ":\n Actual Scene: " + actualSceneName);

            Color newColor = originalMaterial.GetColor ("_EmissionColor");
            Debug.Log (this + ":\n Old Color.G: " + newColor.g);

            newColor = materialToReplaceWith.GetColor ("_EmissionColor");

            originalMaterial.SetColor ("_EmissionColor", newColor);
            Debug.Log (this + ":\n New Color.G: " + newColor.g);
        }
    }
}