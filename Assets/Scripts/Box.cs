using UnityEngine;

public class Box : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out Player player))
        {
            player.PickupBox(this);
            // TODO: add SFX
            Destroy(gameObject);
        }
    }
}
