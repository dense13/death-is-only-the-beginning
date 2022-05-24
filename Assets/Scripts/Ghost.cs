using UnityEngine;

public class Ghost : MonoBehaviour
{
    // Enums
    public enum GhostState { Off, Transitioning, Playing }


    // Properties
    public GhostState State { get; set; } // FUTURE: accessibility


    #region Monobehaviour

    private void Awake() {
        State = GhostState.Off;
    }

    #endregion
}
