using UnityEngine;

public class Ghost : MonoBehaviour, IDamageable
{
    // Enums
    public enum GhostState { Off, Transitioning, Playing }


    // Properties
    public GhostState State { get; set; } // FUTURE: accessibility


    [Header("Cfg")]
    [SerializeField] private float initialTimeBetweenShots = 1f;


    [Header("Setup")]
    [SerializeField] private GameObject shotPrefab;
    [SerializeField] private Transform shotPosition;


    // Private
    private GhostController ghostController;
    private float timeBetweenShots;
    private float timeToNextShot;


    #region Monobehaviour

    private void Awake()
    {
        ghostController = GetComponent<GhostController>();
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

    public void UpgradeSpeed()
    {
        ghostController.UpgradeSpeed();
    }


    public void UpgradeShootingSpeed()
    {
        timeBetweenShots *= .9f; // FUTURE: adjust and limit this
    }

    #endregion


    #region IDamageable

    public void Die()
    {
        Destroy(gameObject);
        // TODO: explode instead of just destroying
        // TODO: show game over UI
    }

    #endregion


    #region Private

    private void Shoot()
    {
        Instantiate(shotPrefab, shotPosition.position, shotPosition.rotation);
    }

    #endregion
}
