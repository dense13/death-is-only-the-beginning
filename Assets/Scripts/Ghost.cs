using UnityEngine;

public class Ghost : MonoBehaviour, IDamageable
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


    #region IDamageable

    public void Die()
    {
        Destroy(gameObject);
        // TODO: explode instead of just destroying
        // TODO: show game over UI
    }

    #endregion
}
