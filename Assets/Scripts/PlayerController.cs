using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Cfg")]
    [SerializeField] private float speed = 5f;

    // Private
    private CharacterController characterController;

    #region Monobehaviour

    private void Awake() {
        characterController = GetComponent<CharacterController>();
    }

    private void Update() {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementVector = new Vector3(horizontalInput, 0, verticalInput);
        characterController.Move(movementVector * Time.deltaTime * speed);
    }

    #endregion
}
