using UnityEngine;

public class Shot : MonoBehaviour
{
    // Properties
    public float Damage = 1f; // FUTURE: accesibility


    [Header("Cfg")]
    [SerializeField] private float speed = 20f;


    #region Monobehaviour

    private void Awake() {
        Destroy(gameObject, 10f);
    }


    private void Update() {
        transform.position = transform.position + Vector3.forward * speed * Time.deltaTime;
    }

    #endregion
}
