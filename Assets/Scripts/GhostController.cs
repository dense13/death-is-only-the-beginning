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
        if (ghost.State != Ghost.GhostState.Playing) return;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 destination = transform.localPosition + new Vector3(horizontalInput, 0, verticalInput);
        if (destination.x < -8.5) destination.x = -8.5f;
        if (destination.x > 8.5) destination.x = 8.5f;
        if (destination.z < -14) destination.z = -14f;
        if (destination.z > 14) destination.z = 14f;
        // FUTURE: the movement cropping is not resolution independent

        transform.localPosition = Vector3.MoveTowards(transform.localPosition, destination, speed * Time.deltaTime);
    }

    #endregion


    #region Public

    public void UpgradeSpeed()
    {
        speed += 1f; // FUTURE: adjust, and limit this
    }

    #endregion
}
