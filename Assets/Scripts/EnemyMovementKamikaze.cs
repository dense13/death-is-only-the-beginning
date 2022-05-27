using UnityEngine;

public class EnemyMovementKamikaze : MonoBehaviour
{
    [Header("Cfg")]
    [SerializeField] private float verticalSpeed = 5f;
    [SerializeField] private float horizontalSpeed = 5f;
    [SerializeField] private float movementDelay = 1f;


    // Private
    private Ghost ghost;
    private Rigidbody rb;
    private int overtakeDirection;


    #region Monobehaviour

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();    
    }

    
    private void Start()
    {
        ghost = FindObjectOfType<Ghost>();
    }


    private void FixedUpdate()
    {
        if (ghost == null)
        {
            enabled = false;
            return;
        }

        if (movementDelay > 0)
        {
            movementDelay -= Time.deltaTime;
            return;
        }


        Vector3 movement = Vector3.back * verticalSpeed;
        if (HasOvertakenGhost())
        {
            movement += Vector3.right * overtakeDirection * horizontalSpeed;
        }
        else
        {
            movement += Vector3.left * SideOfPlayer() * horizontalSpeed;
            overtakeDirection = (ghost.transform.position.x > transform.position.x)? 1 : -1;
        }
        rb.MovePosition(transform.position + movement * Time.deltaTime);
    }

    #endregion


    #region Private
    
    private bool HasOvertakenGhost()
    {
        return transform.position.z < ghost.transform.position.z;
    }


    private int SideOfPlayer()
    {
        // Returns -1 (left), 0 (aligned) or 1 (right)
        if (transform.position.x < ghost.transform.position.x - 0.1f) return -1;
        else if (transform.position.x > ghost.transform.position.x + 0.1f) return 1;
        else return 0;
    }

    #endregion
}
