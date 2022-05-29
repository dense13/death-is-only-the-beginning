using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelManager : MonoBehaviour
{
    // Singleton property
    public static LevelManager I { get; private set; }


    [Header("Config")]
    [SerializeField][Tooltip("In seconds")] private float stageLength = 20f;
    [SerializeField] private int secondsToEndOfHumanity = 10;
    
    
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
    private HashSet<PowerupType> availablePowerups = new HashSet<PowerupType>();
    private bool isInHumanPhase = true;


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

        StartCoroutine(__StartCountdown());
    }


    private void Update()
    {
        if (isInHumanPhase) return;

        // If Ghost is playing
        if (ghost.State == Ghost.GhostState.Playing)
        {
            if (timeToNextStage <= 0)
            {
                stage++;
                Debug.Log("STAGE " + (stage + 1));
                timeToNextStage = stageLength;
            }
            timeToNextStage -= Time.deltaTime;
        }
    }


    private void OnDestroy()
    {
        ghostHealth.OnHealthChange -= ProcessOnHealthChange;        
    }

    #endregion


    #region Public

    public void AddPowerupType(PowerupType powerupType)
    {
        availablePowerups.Add(powerupType);
    }


    public void ProcessEndOfTile(GameObject tile)
    {
        Instantiate(GetRandomTilePrefab(), tile.transform.position + Vector3.forward * tileLength * 2, Quaternion.identity); // FUTURE: 2 is the number of initial tiles
        Destroy(tile, 10f);
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


    private void ShowMessage(string msg)
    {
        Debug.Log(msg);
    }

    #endregion


    #region Coroutines

    private IEnumerator __StartCountdown()
    {
        ShowMessage("Warning, global warming has just reached 4 degrees.");
        yield return new WaitForSeconds(5f);
        ShowMessage("The Earth core has become fatally unstable...");
        yield return new WaitForSeconds(5f);
        ShowMessage("... and the planet will explode in...");
        yield return new WaitForSeconds(5f);
        for (int i = secondsToEndOfHumanity; i >= 0; i--)
        {
            ShowMessage($"... {i} ...");
            yield return new WaitForSeconds(1f);
        }
        ShowMessage("KA-BOOOM!!!!");
        yield return __EndHumanPhase();
    }
    
    
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

        yield return new WaitForSeconds(3f);

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
