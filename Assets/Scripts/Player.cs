using UnityEngine;

public class Player : MonoBehaviour
{
    #region Public

    public void PickupBox(Box box)
    {
        //Debug.Log("Picked up the box " + box.name);

        // TODO: implement this system. Initially I'll just transition to phase 2
        LevelManager.I.EndHumanPhase();
    }

    #endregion
}
