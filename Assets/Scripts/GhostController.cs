using UnityEngine;

public class GhostController : MonoBehaviour
{

    // Private
    private Ghost ghost;


    #region Monobehaviour

    private void Awake() {
        ghost = GetComponent<Ghost>();
    }


    private void Update() {
        if (ghost.State != Ghost.GhostState.Playing) return;

        // TODO: player input and movement
    }

    #endregion
}
