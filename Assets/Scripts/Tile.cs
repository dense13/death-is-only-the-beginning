using UnityEngine;

public class Tile : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out Ghost ghost))
        {
            LevelManager.I.ProcessEndOfTile(this.gameObject);
        }
    }
}
