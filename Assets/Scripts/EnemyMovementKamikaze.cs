using UnityEngine;

public class EnemyMovementKamikaze : MonoBehaviour
{
    [Header("Cfg")]
    [SerializeField] private float speed = 5f;


    // Private
    private Ghost ghost;


    #region Monobehaviour
    
    private void Start()
    {
        ghost = FindObjectOfType<Ghost>();
    }


    private void Update()
    {
        if (ghost == null)
        {
            enabled = false;
            return;
        }

        transform.position = transform.position + CalculateDirection() * speed * Time.deltaTime;
    }

    #endregion


    #region Private

    private Vector3 CalculateDirection()
    {
        return (ghost.transform.position - this.transform.position).normalized;
    }
 
     #endregion
}
