using UnityEngine;

public class Shot : MonoBehaviour
{
    [Header("Cfg")]
    [SerializeField] private float speed = 20f;


    #region Monobehaviour

    private void Update() {
        transform.position = transform.position + Vector3.forward * speed * Time.deltaTime;
    }

    #endregion
}
