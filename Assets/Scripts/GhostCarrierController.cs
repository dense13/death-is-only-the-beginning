using UnityEngine;

public class GhostCarrierController : MonoBehaviour
{
    [Header("Cfg")]
    [SerializeField] private float ascensionSpeed = 3f;
    [SerializeField] private float initialForwardSpeed = 5f;
 

    [Header("Setup")]
    [SerializeField] private WaveSpawner waveSpawner;
    [SerializeField] private PowerupSpawner powerupSpawner;


    // Private
    private Ghost ghost;
    private Vector3 ascensionPoint;
    private bool isAscensionPointSet = false;
    private bool isRunning = false;
    private float forwardSpeed;


    #region Monobehaviour

    private void Awake()
    {
        ghost = GetComponentInChildren<Ghost>();
        waveSpawner.enabled = false;
        powerupSpawner.enabled = false;
        forwardSpeed = initialForwardSpeed;
    }


    private void Update()
    {
        if (ghost.State == GhostState.Off) return;

        if (ghost.State == GhostState.Transitioning)
        {
            if (!isAscensionPointSet)
            {
                ascensionPoint = transform.position + Vector3.up * 50f;
                isAscensionPointSet = true;
            }

            // FUTURE: for now this is manually synced with the camera transition, bad idea
            transform.position = Vector3.MoveTowards(transform.position, ascensionPoint, ascensionSpeed * Time.deltaTime);
        }
        else
        {
            if (!isRunning)
            {
                isRunning = true;
                waveSpawner.enabled = true;
                powerupSpawner.PrepareCollectedPrefabs();
                powerupSpawner.enabled = true;
            }
            transform.position = transform.position + Vector3.forward * (forwardSpeed + LevelManager.I.Stage / 2f) * Time.deltaTime;
        }
    }

    #endregion


    #region Public

    public float GetForwardSpeed()
    {
        return forwardSpeed;
    }

    #endregion

}
