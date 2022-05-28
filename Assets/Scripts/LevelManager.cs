using System.Collections;
using UnityEngine;
using Cinemachine;

public class LevelManager : MonoBehaviour
{
    // Singleton property
    public static LevelManager I { get; private set; }


    [Header("Config")]
    [SerializeField][Tooltip("In seconds")] private float stageLength = 10f;
    
    
    [Header("Setup")]
    [SerializeField] private CinemachineVirtualCamera vcamHuman;
    [SerializeField] private CinemachineVirtualCamera vcamGhost;
    [SerializeField] private GameObject[] tilePrefabs;
    [SerializeField] private UIEndPanel uiEndPanel;


    // Properties
    public int Stage { get { return stage; } }


    // Private
    private Player player;
    private Ghost ghost;
    private Health ghostHealth;
    private Transform ghostCarrierTr;
    private UIHud uiHud;
    private float tileLength = 40f; // FUTURE: this shouldn't be a magic number
    private int score = 0;
    private int stage = 0;
    private float timeToNextStage;

    #region Monobehaviour

    private void Awake()
    {
        SetupSingleton();
        timeToNextStage = stageLength;
    }


    private void Start()
    {
        player = FindObjectOfType<Player>();
        ghost = FindObjectOfType<Ghost>();
        ghostHealth = ghost.GetComponent<Health>();
        ghostCarrierTr = ghost.transform.parent.transform;
        uiHud = FindObjectOfType<UIHud>();
        uiHud.gameObject.SetActive(false);

        player.gameObject.SetActive(true);
        ghost.gameObject.SetActive(false);
        vcamHuman.gameObject.SetActive(true);
        vcamGhost.gameObject.SetActive(false);
        uiEndPanel.gameObject.SetActive(false);

        ghostHealth.OnHealthChange += ProcessOnHealthChange;
    }


    private void Update()
    {
        if (timeToNextStage <= 0)
        {
            stage++;
            timeToNextStage = stageLength;
        }
        timeToNextStage -= Time.deltaTime;
    }


    private void OnDestroy()
    {
        ghostHealth.OnHealthChange -= ProcessOnHealthChange;        
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


    public void EndGhostPhase()
    {
        if (score > GameManager.I.TopScore)
        {
            GameManager.I.TopScore = score;
            uiEndPanel.ShowNewTopScore(); 
        }
        uiEndPanel.gameObject.SetActive(true);
    }


    public void AddScore(int amount)
    {
        if (ghost.State != Ghost.GhostState.Playing) return;

        score += amount;
        uiHud.UpdateScore(score);
    }


    public void UpdateHealth(float currHealth, float totalHealth)
    {
        uiHud.UpdateHealth(currHealth, totalHealth);
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
        uiHud.gameObject.SetActive(true); // FUTURE: fade in
        ghost.State = Ghost.GhostState.Playing;
    }

    #endregion


    #region Action handlers
    
    private void ProcessOnHealthChange(float currHealth, float totalHealth)
    {
        uiHud.UpdateHealth(currHealth, totalHealth);
    }

    #endregion
}
