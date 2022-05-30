using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Cfg")]
    [SerializeField] private float collisionDamage = 5f;
    [SerializeField] private int scoreValue = 1;


    [Header("Setup")]
    [SerializeField] private GameObject vfxExplosion;

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

            ghost.ReceiveHit(collisionDamage);
            Die();
        }
        // Shot
        else if (other.gameObject.TryGetComponent(out Shot shot))
        {
            if (shot.IsEnemyShot) return;
            
            Destroy(shot.gameObject);

            float damage = (LevelManager.I.Stage <= 10) ? shot.Damage : shot.Damage * (10f / (float)LevelManager.I.Stage);
            //Debug.Log($"Damage at Stage {LevelManager.I.Stage} is {damage}");
            health.TakeDamage(damage);
            
            if (health.GetRatio() > 0) GameManager.I.PlaySfx("ENEMY_HURT");
        }
    }

    #endregion


    #region IDamageable

    public void Die()
    {
        isDead = true;
        LevelManager.I.AddScore(scoreValue);
        Instantiate(vfxExplosion, transform.position, Quaternion.identity);
        GameManager.I.PlaySfx("ENEMY_DIE");
        Destroy(gameObject);
    }

    #endregion
}
