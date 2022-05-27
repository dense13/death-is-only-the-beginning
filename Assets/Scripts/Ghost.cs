using UnityEngine;

public class Ghost : MonoBehaviour, IDamageable
{
    // Enums
    public enum GhostState { Off, Transitioning, Playing, Dead }


    // Properties
    public GhostState State { get; set; } // FUTURE: accessibility


    [Header("Cfg")]
    [SerializeField] private float initialTimeBetweenShots = 1f;


    [Header("Setup")]
    [SerializeField] private Health health;
    [SerializeField] private GameObject shotPrefab;
    [SerializeField] private Transform shotPosition;
    [SerializeField] private GameObject modelGO;


    // Private
    private GhostController ghostController;
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
        modelGO.SetActive(false);
        enabled = false;
        State = GhostState.Dead;
        // TODO: explode instead of just hiding
        LevelManager.I.EndGhostPhase();
    }

    #endregion


    #region Private

    private void Shoot()
    {
        Instantiate(shotPrefab, shotPosition.position, shotPosition.rotation);
    }

    #endregion
}
