using UnityEngine;

public class Player : MonoBehaviour
{
    #region Public

    public void PickupBox(PowerupType powerupType)
    {
        LevelManager.I.AddPowerupType(powerupType);
    }

    #endregion
}
