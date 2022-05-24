using UnityEngine;

public class Wave : MonoBehaviour
{
    [Header("Cfg")]
    [SerializeField][Range(0,8)] private float maxHorizontalOffset = 8f;


    // Private
    private Enemy[] enemies;


    #region Monobehaviour

    private void Awake() 
    {
        enemies = GetComponentsInChildren<Enemy>();
        if (maxHorizontalOffset > 0)
        {
            ApplyRandomHorizontalSpawning();
        }
    }

    #endregion


    #region Private

    private void ApplyRandomHorizontalSpawning()
    {
        float horizontalOffset = Random.Range(-maxHorizontalOffset, maxHorizontalOffset);
        foreach (Enemy enemy in enemies)
        {
            Vector3 newSpawnPosition = enemy.transform.position;
            newSpawnPosition.x = newSpawnPosition.x + horizontalOffset;
            enemy.transform.position = newSpawnPosition;
        }
    }
        
    #endregion
}
