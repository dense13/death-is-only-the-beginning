using System.Collections;
using UnityEngine;

public class Ghost : MonoBehaviour, IDamageable
{
    // Enums
    public enum GhostState { Off, Transitioning, Playing, Dead }


    // Properties
    public GhostState State { get; set; } // FUTURE: accessibility


    [Header("Cfg")]
    [SerializeField] private float initialTimeBetweenShots = 1f; // FUTURE: shooting should be in a separate class
    [SerializeField] private float invulnerabilityDuration = 10f;
    [SerializeField] private float invulnerabilitySize = 2f;


    [Header("Setup")]
    [SerializeField] private Health health;
    [SerializeField] private GameObject shotPrefab;
    [SerializeField] private Transform shotPosition;
    [SerializeField] private GameObject modelGO;
    [SerializeField] private GameObject modelInvulnerableGO;


    // Private
    private GhostController ghostController;
    private int numShots = 1;
    private float timeBetweenShots;
    private float timeToNextShot;

    private bool isInvulnerable = false;
    private bool isEndingInvulnerability = false;
    private float remainingInvulnerability = 0;
    private Coroutine crInvulnerabilityWarning;
    float invulnerabilityWarningTime = 3f;


    #region Monobehaviour

    private void Awake()
    {
        ghostController = GetComponent<GhostController>();
        health = GetComponent<Health>();
        State = GhostState.Off;
        timeBetweenShots = initialTimeBetweenShots;
        timeToNextShot = timeBetweenShots;
        modelGO.SetActive(true);
        modelInvulnerableGO.SetActive(false);
        transform.localScale = Vector3.one;
    }


    private void Update()
    {
        if (State != GhostState.Playing) return;

        // Update shooting
        timeToNextShot -= Time.deltaTime;
        if (timeToNextShot < 0)
        {
            timeToNextShot = timeBetweenShots;
            Shoot();
        }

        // Update invulnerability
        if (isInvulnerable) UpdateInvulnerability();
    }


    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.TryGetComponent(out Shot shot))
        {
            if (!shot.IsEnemyShot) return;

            Destroy(shot.gameObject);
            ReceiveHit(shot.Damage);
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


    public void ReceiveHit(float damage)
    {
        if (isInvulnerable) return;

        health.TakeDamage(damage);
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


    public void StartInvulnerability()
    {
        Debug.Log("START INVULNERABILITY");

        isInvulnerable = true;
        isEndingInvulnerability = false;
        remainingInvulnerability = invulnerabilityDuration;
        modelGO.SetActive(false);
        modelInvulnerableGO.SetActive(true);
        transform.localScale = Vector3.one * invulnerabilitySize; // FUTURE: tween (and back at the end of invulnerability)
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


    private void UpdateInvulnerability()
    {
        if (remainingInvulnerability > 0)
        {
            if (remainingInvulnerability <= invulnerabilityWarningTime && !isEndingInvulnerability)
            {
                isEndingInvulnerability = true;
                StartCoroutine(RunInvulnerabilityWarning());
            }
            remainingInvulnerability -= Time.deltaTime;
        }
        else
        {
            // De-activate invulnerability
            isInvulnerable = false;
            isEndingInvulnerability = false;
            transform.localScale = Vector3.one;
            modelGO.SetActive(true);
            modelInvulnerableGO.SetActive(false);
            transform.localScale = Vector3.one;
        }
    }


    private IEnumerator RunInvulnerabilityWarning()
    {
        float remainingWarningTime = invulnerabilityWarningTime;
        bool shouldHide = false;
        float flashingSpeed = 0.25f;
        while (isEndingInvulnerability && remainingWarningTime > 0)
        {
            modelInvulnerableGO.SetActive(!shouldHide);
            shouldHide = !shouldHide;
            remainingWarningTime -= flashingSpeed;
            yield return new WaitForSeconds(flashingSpeed);
        }
    }

    #endregion
}
