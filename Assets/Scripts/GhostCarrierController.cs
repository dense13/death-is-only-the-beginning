using UnityEngine;

public class GhostCarrierController : MonoBehaviour
{
    // FUTURE: SRP - the enemy spawning shouldn't be in this class

    [Header("Cfg")]
    [SerializeField] private float ascensionSpeed = 3f;
    [SerializeField] private float forwardSpeed = 5f;


    [Header("Setup")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform enemySpawnerTr;
    [SerializeField] private float timeBetweenWaves = 3f;


    // Private
    private Ghost ghost;
    private Vector3 ascensionPoint;
    private bool isAscensionPointSet = false;
    private float timeToNextWave;


    #region Monobehaviour

    private void Awake() {
        ghost = GetComponentInChildren<Ghost>();
        timeToNextWave = timeBetweenWaves;
    }


    private void Update() {
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
            if (timeToNextWave <= 0)
            {
                Instantiate(GetRandomEnemyPrefab(), enemySpawnerTr.position, enemySpawnerTr.rotation);
                timeToNextWave = timeBetweenWaves;
            }
            transform.position = transform.position + Vector3.forward * forwardSpeed * Time.deltaTime;
            timeToNextWave -= Time.deltaTime;
        }
    }

    #endregion



    #region Private

    private GameObject GetRandomEnemyPrefab()
    {
        return enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
    }

    #endregion
}
