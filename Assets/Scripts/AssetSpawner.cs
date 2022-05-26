using UnityEngine;

public class AssetSpawner : MonoBehaviour
{    
    [Header("Setup")]
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private float timeBetweenSpawns = 3f;


    // Private
    private float timeToNextSpawn;


    #region Monobehaviour

    private void Awake()
    {
        timeToNextSpawn = timeBetweenSpawns;
    }


    private void Update()
    {
        if (timeToNextSpawn <= 0)
        {
            Instantiate(GetRandomPrefab(), transform.position, transform.rotation);
            timeToNextSpawn = timeBetweenSpawns;
        }
        timeToNextSpawn -= Time.deltaTime;
    }

    #endregion



    #region Private

    private GameObject GetRandomPrefab()
    {
        return prefabs[Random.Range(0, prefabs.Length)];
    }

    #endregion 

}
