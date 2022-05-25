using UnityEngine;

public class EnemyMovementVertical : MonoBehaviour
{
    [Header("Cfg")]
    [SerializeField] private float speed = 0;


    // Private
    private Rigidbody rb;


    #region Monobehaviour
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();    
    }


    private void FixedUpdate()
    {
        if (speed == 0) return;
        
        Vector3 newPosition = Vector3.MoveTowards(transform.position, transform.position + Vector3.back, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);
    }

    #endregion

}
