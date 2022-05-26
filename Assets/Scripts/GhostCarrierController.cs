using UnityEngine;

public class GhostCarrierController : MonoBehaviour
{
    [Header("Cfg")]
    [SerializeField] private float ascensionSpeed = 3f;
    [SerializeField] private float initialForwardSpeed = 5f;


    [Header("Setup")]
    [SerializeField] private AssetSpawner enemySpawner;
    [SerializeField] private AssetSpawner powerupSpawner;


    // Private
    private Ghost ghost;
    private Vector3 ascensionPoint;
    private bool isAscensionPointSet = false;
    private float forwardSpeed;


    #region Monobehaviour

    private void Awake()
    {
        ghost = GetComponentInChildren<Ghost>();
        enemySpawner.enabled = false;
        powerupSpawner.enabled = false;
        forwardSpeed = initialForwardSpeed;
    }


    private void Update()
    {
        if (ghost.State == Ghost.GhostState.Off) return;

        if (ghost.State == Ghost.GhostState.Transitioning)
        {
            if (!isAscensionPointSet)
            {
                ascensionPoint = transform.position + Vector3.up * 10f;
                isAscensionPointSet = true;
            }

            // FUTURE: for now this is manually synced with the camera transition, bad idea
            transform.position = Vector3.MoveTowards(transform.position, ascensionPoint, ascensionSpeed * Time.deltaTime);
        }
        else
        {
            enemySpawner.enabled = true;
            powerupSpawner.enabled = true;
            transform.position = transform.position + Vector3.forward * forwardSpeed * Time.deltaTime;
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
