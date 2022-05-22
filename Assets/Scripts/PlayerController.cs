using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float playerSpeed = 2f;

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
        characterController.Move(movementVector * Time.deltaTime * playerSpeed);
    }

    #endregion
}
