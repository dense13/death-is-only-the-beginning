using UnityEngine;

public class Shot : MonoBehaviour
{
    // Properties
    public float Damage = 1f; // FUTURE: accesibility


    [Header("Cfg")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private bool isEnemyShot = false;
    [Header("Homing (ENEMY ONLY)")]
    [SerializeField] private bool isHoming = false;
    [SerializeField] private float maxHorizontalSpeed = 3f;
    [SerializeField] private float horizontalAcceleration = 2f;


    // Properties
    public bool IsEnemyShot { get { return isEnemyShot; } }


    // Private
    private Transform target;
    private float horizontalSpeed = 0;


    #region Monobehaviour

    private void Awake()
    {
        Destroy(gameObject, 10f); // FUTURE: this is a quick and dirty fix, but would fail if a shot can last more than 10s
    }


    private void Start()
    {
        if (isHoming)
        {
            if (isEnemyShot)
            {
                Ghost ghost = FindObjectOfType<Ghost>();
                if (ghost == null) return;
                
                target = ghost.transform;
            }
        }
    }


    private void Update() {
        if (isHoming && target != null)
        {
            if (isEnemyShot)
            {
                horizontalSpeed = Mathf.MoveTowards(horizontalSpeed, maxHorizontalSpeed * DirectionToTarget(), horizontalAcceleration * Time.deltaTime);
                Vector3 movement = Vector3.back * speed + Vector3.right * horizontalSpeed;
                transform.position = transform.position + movement * Time.deltaTime;
            }
            else
            {
                 // TODO: homing Player shots. For now, just move them as if not homing
                transform.position = transform.position + transform.forward * speed * Time.deltaTime;
            }
        }
        else
        {
            transform.position = transform.position + transform.forward * speed * Time.deltaTime;
        }
    }


    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("StopsBullets"))
        {
            Destroy(gameObject);
        }
    }




    #endregion



    #region Private
    
    private int DirectionToTarget()
    {
        const float BUFFER = 0.2f;
        if (transform.position.x < target.position.x - BUFFER) return 1;
        else if (transform.position.x > target.position.x + BUFFER) return -1;
        else return 0;
    }
    #endregion
}
