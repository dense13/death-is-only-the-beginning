using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Actions
    public event Action<float, float> OnHealthChange;


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
        OnHealthChange?.Invoke(health, initialHealth);
        
        if (health <= 0)
        {
            health = 0;
            owner.Die();
        }
    }


    public bool Heal(float amount)
    {
        // Returns false if health was already at max
        if (health == initialHealth) return false;

        health += amount;
        if (health > initialHealth) health = initialHealth;
        OnHealthChange?.Invoke(health, initialHealth);
        return true;
    }


    public float GetRatio()
    {
        return health / initialHealth;
    }


    #endregion
}
