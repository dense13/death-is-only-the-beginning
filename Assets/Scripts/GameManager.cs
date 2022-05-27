using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton property
    public static GameManager I { get; private set; }


    // Properties
    public int TopScore = 0;


    #region Monobehaviour

    private void Awake()
    {
        SetupSingleton();
    }

    #endregion


    #region Private

    private void SetupSingleton()
    {
        // Singleton setup
        if (I != null && I != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            I = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    #endregion
}
