using UnityEngine;

public class Powerup : MonoBehaviour
{
    private enum PowerupType { Speed, ShootingSpeed }

    [Header("Cfg")]
    [SerializeField] private PowerupType type = PowerupType.Speed;


    // Private
    private bool hasBeenUsed = false;


    #region Monobehaviour

    private void OnTriggerEnter(Collider other)
    {
        if (hasBeenUsed) return;

        if (other.TryGetComponent(out Ghost ghost))
        {
            switch (type)
            {
                case PowerupType.Speed:
                    ghost.UpgradeSpeed();
                    break;
                case PowerupType.ShootingSpeed:
                    ghost.UpgradeShootingSpeed();
                    break;
                default:
                    Debug.LogError("OOPS! Unrecognized powerup type " + type);
                    break;
            }
            hasBeenUsed = true;
            Destroy(gameObject); // FUTURE: SFX, instead of just destroying
        }
    }

    #endregion

}
