using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Cfg")]
    [SerializeField] private float initialHealth = 10f;


    // Private
    private float health;
    private IDamageable owner;


    #region Monobehaviour

    private void Awake() {
        health = initialHealth;
        owner = GetComponent<IDamageable>();
        if (owner == null)
        {
            Debug.LogError($"{name} has a Health but no IDamageable");
        }
    }

    #endregion



    #region Public

    public void TakeDamage(float damage)
    {
        if (health == 0) return;

        health -= damage;
        if (health <= 0)
        {
            health = 0;
            owner.Die();
        }
    }

    #endregion
}
