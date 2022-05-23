using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Singleton property
    public static LevelManager I { get; private set; }

    // Private
    private Player player;
    private Ghost ghost;


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
        ghost = FindObjectOfType<Ghost>();
        ghost.gameObject.SetActive(false);
    }

    #endregion


    #region Public

    public void EndHumanPhase()
    {
        player.gameObject.SetActive(false);
        ghost.transform.position = player.transform.position;
        //ghost.transform.rotation = player.transform.rotation;
        ghost.gameObject.SetActive(true);
        Debug.Log("TODO: Level.EndHumanPhase");
    }

    #endregion
}
