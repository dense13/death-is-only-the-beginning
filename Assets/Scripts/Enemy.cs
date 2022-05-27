using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Cfg")]
    [SerializeField] private float collisionDamage = 5f;
    [SerializeField] private int scoreValue = 1;

    // Private
    private bool isDead = false;
    private Health health;
    
    
    #region Monobehaviour

    private void Awake()
    {
        health = GetComponent<Health>();
    }


    private void OnCollisionEnter(Collision other) {
        if (isDead) return;
        
        // Ghost
        if (other.gameObject.TryGetComponent(out Ghost ghost))
        {

            ghost.GetComponent<Health>().TakeDamage(collisionDamage);
            Die();
        }
        // Shot
        else if (other.gameObject.TryGetComponent(out Shot shot))
        {
            Destroy(shot.gameObject);
            health.TakeDamage(shot.Damage);
        }
    }

    #endregion


    #region IDamageable

    public void Die()
    {
        // TODO: explode instead of just destroying
        isDead = true;
        LevelManager.I.AddScore(scoreValue);
        Destroy(gameObject);
        // TODO: explode instead of just destroying
    }

    #endregion
}
