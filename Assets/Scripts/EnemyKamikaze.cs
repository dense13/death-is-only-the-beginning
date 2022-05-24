using UnityEngine;

public class EnemyKamikaze : MonoBehaviour
{
    [Header("Cfg")]
    [SerializeField] private float speed = 4f;


    // Private
    private Ghost ghost;


    #region Monobehaviour
    
    private void Start() {
        ghost = FindObjectOfType<Ghost>();
    }


    private void Update() {
        if (ghost == null)
        {
            enabled = false;
            return;
        }
        
        transform.position = Vector3.MoveTowards(transform.position, ghost.transform.position, speed * Time.deltaTime);
    }

    #endregion
}
