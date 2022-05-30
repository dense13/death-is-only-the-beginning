using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{    
    [Header("Setup")]
    [SerializeField][Tooltip("HACK! Must be in the order of the enum!")] private GameObject[] allPrefabs;
    [SerializeField] private float timeBetweenSpawns = 20f;


    // Private
    private float timeToNextSpawn;
    private List<GameObject> prefabs = new List<GameObject>();


    #region Monobehaviour

    private void Awake()
    {
        timeToNextSpawn = timeBetweenSpawns;
    }


    private void Update()
    {

        if (timeToNextSpawn <= 0)
        {
            Instantiate(GetRandomPrefab(), transform.position + Vector3.left * (Random.Range(-6f, 6f)), transform.rotation);
            timeToNextSpawn = timeBetweenSpawns;
        }
        timeToNextSpawn -= Time.deltaTime;
    }

    #endregion



    #region Public
        
    public void PrepareCollectedPrefabs()
    {
        foreach (PowerupType powerupType in LevelManager.I.AvailablePowerups)
        {
            prefabs.Add(allPrefabs[(int)powerupType]);
        }
        if (prefabs.Count == 0) gameObject.SetActive(false);
    }

    #endregion



    #region Private

    private GameObject GetRandomPrefab()
    {
        return prefabs[Random.Range(0, prefabs.Count)];
    }

    #endregion 

}
