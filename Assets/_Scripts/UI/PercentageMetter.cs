using TMPro;
using UnityEngine;

namespace _Scripts.UI {
    public class PercentageMetter : MonoBehaviour {

        private TextMeshProUGUI percentageTmp;

        private void Start() {  
            percentageTmp = this.gameObject.GetComponent<TextMeshProUGUI>();
        }

        public void UpdateText (float actualValue) {
            percentageTmp.text = actualValue.ToString("0.0") + "%";
        }
    }
}