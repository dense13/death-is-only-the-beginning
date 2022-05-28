using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private PrefabCollection[] waveSets;
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
            PrefabCollection waveSet = GetWaveSet();
            Instantiate(waveSet.GetRandom(), transform.position, transform.rotation);
            timeToNextSpawn = timeBetweenSpawns;
        }
        timeToNextSpawn -= Time.deltaTime;
    }

    #endregion


    #region Private
    
    private PrefabCollection GetWaveSet()
    {
        return waveSets[Mathf.Min(waveSets.Length - 1, LevelManager.I.Stage)];
    }

    #endregion
}
