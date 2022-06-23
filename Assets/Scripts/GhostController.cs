using UnityEngine;

public class GhostController : MonoBehaviour
{

    [Header("Cfg")]
    [SerializeField] private float initialSpeed = 6f;

    // Private
    private Ghost ghost;
    private float speed;


    #region Monobehaviour

    private void Awake()
    {
        ghost = GetComponent<Ghost>();
        speed = initialSpeed;
    }


    private void Update()
    {
        if (ghost.State != GhostState.Playing) return;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 destination = transform.localPosition + new Vector3(horizontalInput, 0, verticalInput);
        if (destination.x < -8.5) destination.x = -8.5f;
        if (destination.x > 8.5) destination.x = 8.5f;
        if (destination.z < -13) destination.z = -13f;
        if (destination.z > 18) destination.z = 18f;
        // FUTURE: the movement cropping is not resolution independent

        transform.localPosition = Vector3.MoveTowards(transform.localPosition, destination, speed * Time.deltaTime);
    }

    #endregion


    #region Public

    public bool UpgradeSpeed()
    {
        // Returns false if speed has reached a top threshold
        if (speed >= 10f) return false;

        speed += 1f;
        return true;
    }

    #endregion
}
