using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float timeBetweenWaves = 3f;


    // Private
    private float timeToNextWave;


    #region Monobehaviour

    private void Awake()
    {
        timeToNextWave = timeBetweenWaves;
    }


    private void Update()
    {
        if (timeToNextWave <= 0)
        {
            Instantiate(GetRandomEnemyPrefab(), transform.position, transform.rotation);
            timeToNextWave = timeBetweenWaves;
        }
        timeToNextWave -= Time.deltaTime;
    }

    #endregion



    #region Private

    private GameObject GetRandomEnemyPrefab()
    {
        return enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
    }

    #endregion 

}
