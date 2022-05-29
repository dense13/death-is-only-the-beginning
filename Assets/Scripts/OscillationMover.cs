using UnityEngine;

public class OscillationMover : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private Vector3 movementVector = new Vector3(0, 1f, 0); // this needs to be written in Inspector
    [SerializeField] private float speed = 2f;
    [SerializeField] private float startDelay = -1f; // -1: random
    [SerializeField] private bool useWorldSpace;

    // Private
    private Vector3 startingPos;
    private float startTime;
    private bool hasStarted = false;



    void Start()
    {
        startingPos = transform.position;
        startTime = Time.time;
        if (startDelay < 0) startDelay = Random.Range(0, 2f);
    }


    void Update()
    {
        // Wait delay
        if (!hasStarted)
        {
            if (Time.time - startTime < startDelay)
            {
                return;
            }
            else // this will only happen once, after startDelay elapses
            {
                hasStarted = true;
                startTime = Time.time;
            }
        }

        Move();
    }


    private void Move()
    {
        Vector3 amountToMove;

        if (useWorldSpace)
        {
            amountToMove = movementVector;
        }
        else
        {
            amountToMove = transform.right * movementVector.x + transform.up * movementVector.y + transform.forward * movementVector.z;
        }

        amountToMove *= -Mathf.Cos((Time.time - startTime) * speed) / 2f + 0.5f;
        transform.position = startingPos + amountToMove;
    }
}
