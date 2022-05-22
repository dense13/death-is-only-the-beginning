using UnityEngine;

public class GhostController : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float ascensionSpeed = 3f;


    #region Monobehaviour

    private void Update() {
        // TODO: this is just placeholder code
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, 10f, 0), ascensionSpeed * Time.deltaTime);
    }

    #endregion
}
