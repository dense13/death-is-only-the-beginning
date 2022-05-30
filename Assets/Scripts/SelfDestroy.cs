using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [Header("Cfg")]
    [SerializeField] private float lifetime = 6f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
