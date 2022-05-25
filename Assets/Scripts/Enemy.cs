using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Cfg")]
    [SerializeField] private float collisionDamage = 5f;

    // Private
    private bool hasExploded = false;
    private Health health;
    
    
    #region Monobehaviour

    private void Awake()
    {
        health = GetComponent<Health>();
    }


    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.TryGetComponent(out Ghost ghost))
        {
            if (hasExploded) return;

            ghost.GetComponent<Health>().TakeDamage(collisionDamage);
            // TODO: explode instead of just destroying
            Destroy(gameObject);
            hasExploded = true;
        }
        else if (other.gameObject.TryGetComponent(out Shot shot))
        {
            health.TakeDamage(shot.Damage);
            Destroy(shot.gameObject);
        }
    }

    #endregion


    #region IDamageable

    public void Die()
    {
        Destroy(gameObject);
        // TODO: explode instead of just destroying
    }

    #endregion
}
