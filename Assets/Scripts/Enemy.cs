using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Cfg")]
    [SerializeField] private float collisionDamage = 5f;

    // Private
    private bool hasExploded = false;
    
    
    #region Monobehaviour

    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out Ghost ghost))
        {
            if (hasExploded) return;

            ghost.GetComponent<Health>().TakeDamage(collisionDamage);
            // TODO: explode instead of just destroying
            Destroy(gameObject);
            hasExploded = true;
        }
    }

    #endregion
}
