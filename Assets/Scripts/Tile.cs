using UnityEngine;

public class Tile : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out GhostCarrierController _))
        {
            LevelManager.I.ProcessEndOfTile(this.gameObject);
        }
    }
}
