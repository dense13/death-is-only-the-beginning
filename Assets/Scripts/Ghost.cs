using UnityEngine;

public class Ghost : MonoBehaviour, IDamageable
{
    // Enums
    public enum GhostState { Off, Transitioning, Playing, Dead }


    // Properties
    public GhostState State { get; set; } // FUTURE: accessibility


    [Header("Cfg")]
    [SerializeField] private float initialTimeBetweenShots = 1f; // FUTURE: shooting should be in a separate class


    [Header("Setup")]
    [SerializeField] private Health health;
    [SerializeField] private GameObject shotPrefab;
    [SerializeField] private Transform shotPosition;


    // Private
    private GhostController ghostController;
    private int numShots = 1;
    private float timeBetweenShots;
    private float timeToNextShot;


    #region Monobehaviour

    private void Awake()
    {
        ghostController = GetComponent<GhostController>();
        health = GetComponent<Health>();
        State = GhostState.Off;
        timeBetweenShots = initialTimeBetweenShots;
        timeToNextShot = timeBetweenShots;
    }


    private void Update()
    {
        if (State != GhostState.Playing) return;

        timeToNextShot -= Time.deltaTime;
        if (timeToNextShot < 0)
        {
            timeToNextShot = timeBetweenShots;
            Shoot();
        }
    }


    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.TryGetComponent(out Shot shot))
        {
            if (!shot.IsEnemyShot) return;

            Destroy(shot.gameObject);
            health.TakeDamage(shot.Damage);
        }
    }

    #endregion


    #region Public

    public bool UpgradeSpeed()
    {
        return ghostController.UpgradeSpeed();
    }


    public bool UpgradeShootingSpeed()
    {
        // Returns false if time has reach a low threshold
        if (timeBetweenShots <= 0.1f) return false;

        timeBetweenShots -= 0.1f;
        return true;
    }


    public bool UpgradeMultishot()
    {
        // Returns false if max num shots are reached
        if (numShots == 5) return false;
        
        numShots++;
        return true;
    }


    public bool Heal()
    {
        return health.Heal(1f);
    }


    public void TriggerExplosion()
    {
        // FUTURE: this shouldn't be in Ghost, and probably not use FindObjectsOfType, but for the Game Jam that'll do. :)
        // FUTURE: do it only over a certain radious?
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (var enemy in enemies)
        {
            enemy.Die();
        }
    }

    #endregion


    #region IDamageable

    public void Die()
    {
        ghostController.enabled = false;
        gameObject.SetActive(false);
        State = GhostState.Dead;
        // TODO: explode instead of just hiding
        LevelManager.I.EndGhostPhase();
    }

    #endregion


    #region Private

    private void Shoot()
    {
        bool evenShots = (numShots % 2 == 0);
        if (evenShots)
        {
            const float SHOT_SEPARATION = 0.5f;
            float leftOffset = SHOT_SEPARATION * (numShots / 2 - 0.5f);
            for (int i = 0; i < numShots; i++)
            {
                Vector3 position = shotPosition.position + Vector3.right * i * SHOT_SEPARATION + Vector3.left * leftOffset;
                Instantiate(shotPrefab, position, shotPosition.rotation);
            }
        }
        else
        {
            const float SHOT_SPREAD = 5f;
            int numSideShots = (numShots - 1) / 2;
            for (int i = -numSideShots; i <= numSideShots; i++)
            {
                Vector3 rotation = Vector3.forward + Vector3.up * SHOT_SPREAD * i; // FUTURE: not sure why it's Vector3.up, I thought it should've been Vector3.right...
                Instantiate(shotPrefab, shotPosition.position, Quaternion.Euler(rotation));
            }
        }
    }

    #endregion
}
