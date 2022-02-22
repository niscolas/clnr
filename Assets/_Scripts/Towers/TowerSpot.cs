using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.Towers {
    public class TowerSpot : MonoBehaviour {

        [SerializeField]
        private Transform placePoint;

        [SerializeField]
        private Renderer[] renderers;

        private BuildingManager bM;

        private GameManager gM;

        private Tower placedTower;

        private bool occupied = false;

        private void Awake () {
            bM = FindObjectOfType<BuildingManager> ();
            gM = FindObjectOfType<GameManager> ();
        }

        private void OnMouseOver () {
            if (Input.GetMouseButtonUp (0)) {
                OnMouseLeftClickUp ();
            } else if (Input.GetMouseButtonUp (1)) {
                OnMouseRightClickUp ();
            }
        }

        private void OnMouseLeftClickUp () {
            Debug.Log ("TowerSpot: Left Click on Tower Spot");
            // If the Tower Spot is free, place a tower
            if (!occupied) {
                PlaceTower (bM.SelectedTower);
            } else {
                if (placedTower.NextLevelPrefab != null) {
                    UpgradeTower ();
                } else {
                    RemoveTower ();
                }
            }
        }

        private void OnMouseRightClickUp () {
            Debug.Log ("TowerSpot: Right Click on Tower Spot");
            RemoveTower ();
        }

        private bool PlaceTower (Tower towerToPlace) {
            Debug.Log ("TowerSpot: Placing Tower");

            if (occupied) {
                Debug.Log ("TowerSpot: Trying to Upgrade Tower");
            }

            if (gM.TryToChangeMemoryUse (towerToPlace.MemoryCost)) {
                // If the tower is being upgraded
                if (occupied) {
                    Destroy (placedTower.gameObject);
                }

                // Instantiate new Tower 
                placedTower = Instantiate (
                    towerToPlace,
                    placePoint.position,
                    placePoint.rotation);

                placedTower.transform.SetParent (placePoint, true);

                // Now the TowerSpot has a Tower on it
                occupied = true;

                UpdateMaterial ();

                return true;
            } else {
                Debug.Log ("TowerSpot: Not enough Memory Avaiable!");

                return false;
            }
        }

        private void UpgradeTower () {
            Debug.Log ("Upgrading Tower");

            Tower nextLevelTower = placedTower.NextLevelPrefab;

            PlaceTower (nextLevelTower);
        }

        private void RemoveTower () {
            Debug.Log ("TowerSpot: Removing Tower");

            // Removing a Tower, frees 'GameManager.memoryRegainPercentage'%
            gM.TryToChangeMemoryUse (-placedTower.MemoryCost);

            Destroy (placedTower.gameObject);

            // The Tower Spot is now free
            occupied = false;

            UpdateMaterial (false);
        }

        private void UpdateMaterial (bool occupied = true) {
            Material materialToUpdate;

            if (occupied) {
                materialToUpdate = bM.UnavailableMaterial;
            } else {
                materialToUpdate = bM.AvailableMaterial;
            }

            foreach (Renderer r in renderers) {
                r.material = materialToUpdate;
            }
        }
    }
}