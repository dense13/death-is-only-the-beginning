using UnityEngine;

public class Box : MonoBehaviour
{
    [Header("Cfg")]
    [SerializeField] private PowerupType powerupType;


    [Header("Setup")]
    [SerializeField] private GameObject vfxExplosion;



    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out Player player))
        {
            player.PickupBox(powerupType);
            Instantiate(vfxExplosion, transform.position, Quaternion.identity);
            GameManager.I.PlaySfx("POWERUP", 0.3f);
            Destroy(gameObject);
        }
    }
}
