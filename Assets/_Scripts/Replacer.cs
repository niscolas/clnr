using UnityEngine;

namespace _Scripts {
    public class Replacer : MonoBehaviour {

        [SerializeField]
        private Transform replacePoint;

        private void OnTriggerStay (Collider other) {
            Replace (other);
        }

        private void Replace (Collider other) {
            if (other.gameObject.TryGetComponent (out Player player)) {
                Debug.Log ("The Player has collided on the infinity fall collider");
                player.gameObject.transform.position = replacePoint.position;
            }
        }
    }
}