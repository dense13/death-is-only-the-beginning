using UnityEngine;

public class EnemyMovementVertical : MonoBehaviour
{
    [Header("Cfg")]
    [SerializeField][Tooltip("-1 means static")] private float speed = -1;


    #region Monobehaviour
    
    private void Start()
    {
        if (speed < 0)
        {
            speed = FindObjectOfType<GhostCarrierController>().GetForwardSpeed(); // this makes it appear 'static', as if glued to the tiles
        }
    }


    private void Update()
    {
        transform.position = transform.position + Vector3.back * speed * Time.deltaTime;
    }

    #endregion

}
