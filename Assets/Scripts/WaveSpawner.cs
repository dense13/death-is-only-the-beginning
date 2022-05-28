using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private PrefabCollection[] waveSets;
    [SerializeField] private float initialTimeBetweenSpawns = 6f;


    // Private
    private float timeToNextSpawn;


    #region Monobehaviour

    private void Awake()
    {
        timeToNextSpawn = initialTimeBetweenSpawns;
    }


    private void Update()
    {
        if (timeToNextSpawn <= 0)
        {
            PrefabCollection waveSet = GetWaveSet();
            Instantiate(waveSet.GetRandom(), transform.position, transform.rotation);
            timeToNextSpawn = Mathf.Max(1f, initialTimeBetweenSpawns - LevelManager.I.Stage / 3f);
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
