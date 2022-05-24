using UnityEngine;

public class Tile : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out GhostCarrierController _))
        {
            Debug.Log("And it was a GhostCarrierController!");
            LevelManager.I.ProcessEndOfTile(this.gameObject);
        }
    }
}
