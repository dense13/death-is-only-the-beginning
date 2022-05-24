using System.Collections;
using UnityEngine;
using Cinemachine;

public class LevelManager : MonoBehaviour
{
    // Singleton property
    public static LevelManager I { get; private set; }


    [Header("Cfg")]
    [SerializeField] private CinemachineVirtualCamera vcamHuman;
    [SerializeField] private CinemachineVirtualCamera vcamGhost;
    [SerializeField] private GameObject[] tilePrefabs;


    // Private
    private Player player;
    private Ghost ghost;
    private Transform ghostCarrierTr;
    private float tileLength = 40f; // FUTURE: this shouldn't be a magic number


    #region Monobehaviour

    private void Awake()
    {
        SetupSingleton();
    }


    private void Start() {
        player = FindObjectOfType<Player>();
        ghost = FindObjectOfType<Ghost>();
        ghostCarrierTr = ghost.transform.parent.transform;

        player.gameObject.SetActive(true);
        ghost.gameObject.SetActive(false);
        vcamHuman.gameObject.SetActive(true);
        vcamGhost.gameObject.SetActive(false);
    }

    #endregion


    #region Public

    public void ProcessEndOfTile(GameObject tile)
    {
        Instantiate(GetRandomTilePrefab(), tile.transform.position + Vector3.forward * tileLength * 2, Quaternion.identity); // FUTURE: 2 is the number of initial tiles
        Destroy(tile, 10f);
    }


    public void EndHumanPhase()
    {
        StartCoroutine(__EndHumanPhase());
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
        }
    }


    private GameObject GetRandomTilePrefab()
    {
        return tilePrefabs[Random.Range(0, tilePrefabs.Length)];
    }

    #endregion


    #region Coroutines

    private IEnumerator __EndHumanPhase()
    {
        // Place first tile
        Vector3 tilePosition = player.transform.position + Vector3.up * 10f + Vector3.forward * 5f; // FUTURE: this offset shouldn't be magic numbers
        Instantiate(GetRandomTilePrefab(), tilePosition, Quaternion.identity);
        Instantiate(GetRandomTilePrefab(), tilePosition + Vector3.forward * tileLength, Quaternion.identity);

        // Replace model
        player.gameObject.SetActive(false);
        ghostCarrierTr.position = player.transform.position + Vector3.back * ghost.transform.position.z;
        ghost.gameObject.SetActive(true);

        // Transition camera
        ghost.State = Ghost.GhostState.Transitioning;
        vcamHuman.gameObject.SetActive(false);
        vcamGhost.gameObject.SetActive(true);
        yield return new WaitForSeconds(4f); // FUTURE: this is the length of the vcam transitions

        // Start Ghost phase
        ghost.State = Ghost.GhostState.Playing;
    }

    #endregion
}
