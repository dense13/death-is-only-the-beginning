using UnityEngine;

public class Box : MonoBehaviour
{
    [Header("Cfg")]
    [SerializeField] private PowerupType powerupType;


    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out Player player))
        {
            player.PickupBox(powerupType);
            // TODO: add SFX
            Destroy(gameObject);
        }
    }
}
