using UnityEngine;

public class GhostCarrierController : MonoBehaviour
{
    [Header("Cfg")]
    [SerializeField] private float ascensionSpeed = 3f;
    [SerializeField] private float initialForwardSpeed = 5f;


    // Private
    private Ghost ghost;
    private Vector3 ascensionPoint;
    private EnemySpawner enemySpawner;
    private bool isAscensionPointSet = false;
    private float forwardSpeed;


    #region Monobehaviour

    private void Awake()
    {
        ghost = GetComponentInChildren<Ghost>();
        enemySpawner = GetComponentInChildren<EnemySpawner>();
        enemySpawner.enabled = false;
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
