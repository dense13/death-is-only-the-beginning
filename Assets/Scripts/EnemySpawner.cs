using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject[] wavePrefabs;
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
            Instantiate(GetRandomWavePrefab(), transform.position, transform.rotation, transform);
            timeToNextWave = timeBetweenWaves;
        }
        timeToNextWave -= Time.deltaTime;
    }

    #endregion



    #region Private

    private GameObject GetRandomWavePrefab()
    {
        return wavePrefabs[Random.Range(0, wavePrefabs.Length)];
    }

    #endregion 

}
