using UnityEngine;

public class EnemyKamikaze : MonoBehaviour
{
    [Header("Cfg")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float slowdownThreshold = 20f;


    // Private
    private Ghost ghost;
    private GhostCarrierController ghostCarrierController;


    #region Monobehaviour
    
    private void Start()
    {
        ghost = FindObjectOfType<Ghost>();
        ghostCarrierController = FindObjectOfType<GhostCarrierController>();
    }


    private void Update()
    {
        if (ghost == null)
        {
            enabled = false;
            return;
        }

        //transform.position = Vector3.MoveTowards(transform.position, ghost.transform.position, CalculateSpeed() * Time.deltaTime);
        transform.position = transform.position + CalculateDirection() * speed * Time.deltaTime;
    }

    #endregion


    #region Private

    private Vector3 CalculateDirection()
    {
        return (ghost.transform.position - this.transform.position).normalized;
    }
    
    // private float CalculateSpeed()
    // {
    //     // TODO: this is not good enough. What I have to do:
    //     // - Horizontal movement towards the player, at a speed that might change based on thresold
    //     // - (!) Calculate vertical movement, taking into consideration carrier AND player speed, and wether I'm above or below the player
    //     float sqrDistanceToGhost = Vector3.SqrMagnitude(ghost.transform.position - this.transform.position);
    //     if (sqrDistanceToGhost < slowdownThreshold)
    //     {
    //         return ghost.GetSpeed() + 1f;
    //     }
    //     else
    //     {
    //         return ghost.GetSpeed() + relSpeed;
    //     }
    // }


    // private bool IsEnemyBelowGhost()
    // {
    //     return (transform.position.z < ghost.transform.position.z);
    // }

    #endregion
}
