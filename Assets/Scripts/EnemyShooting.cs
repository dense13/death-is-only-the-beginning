using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [Header("Cfg")]
    [SerializeField] private float timeBetweenShots = 2f;


    [Header("Setup")]
    [SerializeField] private GameObject shotPrefab;
    [SerializeField] private Transform shootingPositionsContainer;


    // Private
    private float timeToNextShot;


    #region Monobehaviour
    
    private void Awake() 
    {
        timeToNextShot = timeBetweenShots;
    }


    private void Update() {
        timeToNextShot -= Time.deltaTime;
        if (timeToNextShot < 0)
        {
            Shoot();
            timeToNextShot = timeBetweenShots;
        }
    }

    #endregion



    #region Private
    
    private void Shoot()
    {
        foreach (Transform shootingPositionTr in shootingPositionsContainer)
        {
            Instantiate(shotPrefab, shootingPositionTr.position, shootingPositionTr.rotation);            
        }
        GameManager.I.PlaySfx("ENEMY_SHOT");
    }

    #endregion
}
