using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Singleton property
    public static LevelManager I { get; private set; }

    // Private
    private Player player;


    #region Monobehaviour

    private void Awake()
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
        }
        DontDestroyOnLoad(this.gameObject);
    }


    private void Start() {
        player = FindObjectOfType<Player>();
    }

    #endregion


    #region Public

    public void EndHumanPhase()
    {
        player.gameObject.SetActive(false);
        Debug.Log("TODO: Level.EndHumanPhase");
    }

    #endregion
}
