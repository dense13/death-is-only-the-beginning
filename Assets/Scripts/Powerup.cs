using UnityEngine;

public enum PowerupType { Bomb, Health, Invulnerability, Multishot, Speed , ShootingSpeed }

public class Powerup : MonoBehaviour
{

    private const int SCORE_FOR_UNUSABLE_POWERUP = 10;

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
                    if (!ghost.UpgradeSpeed()) UnusablePowerup();
                    break;
                case PowerupType.ShootingSpeed:
                    if (!ghost.UpgradeShootingSpeed()) UnusablePowerup();
                    break;
                case PowerupType.Health:
                    if (!ghost.Heal()) UnusablePowerup();
                    break;
                case PowerupType.Bomb:
                    ghost.TriggerExplosion();
                    break;
                case PowerupType.Multishot:
                    if (!ghost.UpgradeMultishot()) UnusablePowerup();
                    break;
                case PowerupType.Invulnerability:
                    ghost.StartInvulnerability();
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


    #region Private
    
    private void UnusablePowerup()
    {
        LevelManager.I.AddScore(SCORE_FOR_UNUSABLE_POWERUP); // FUTURE: show some visual and/or aural feed back
    }
    #endregion

}
