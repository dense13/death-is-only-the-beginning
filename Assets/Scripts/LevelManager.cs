using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    // Singleton property
    public static LevelManager I { get; private set; }


    [Header("Config")]
    [SerializeField][Tooltip("In seconds")] private float stageLength = 20f;
    [SerializeField] private int secondsToEndOfHumanity = 10;
    
    
    [Header("Setup")]
    [SerializeField] private CinemachineVirtualCamera vcamHuman;
    [SerializeField] private CinemachineVirtualCamera vcamTraveling;
    [SerializeField] private CinemachineVirtualCamera vcamGhost;
    [SerializeField] private GameObject[] tilePrefabs;
    [SerializeField] private GameObject vfxExplosion;
    [SerializeField] private UIEndPanel uiEndPanel;
    [SerializeField] private Canvas uiMsgCanvas;
    [SerializeField] private TMP_Text txtMsg;
    


    // Properties
    public int Stage { get { return stage; } }
    public HashSet<PowerupType> AvailablePowerups { get { return availablePowerups; } }


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


    #region Monobehaviour

    private void Awake()
    {
        SetupSingleton();
        timeToNextStage = stageLength;
        uiMsgCanvas.gameObject.SetActive(false);
    }


    private void Start()
    {
        player = FindObjectOfType<Player>();
        ghost = FindObjectOfType<Ghost>();
        ghostHealth = ghost.GetComponent<Health>();
        ghostCarrierTr = ghost.transform.parent.transform;
        uiHud = FindObjectOfType<UIHud>();
        uiHud.gameObject.SetActive(false);

        ghostHealth.OnHealthChange += ProcessOnHealthChange;
        Cursor.lockState = CursorLockMode.Locked;
        
        player.gameObject.SetActive(true);
        ghost.gameObject.SetActive(false);
        vcamHuman.gameObject.SetActive(true);
        vcamTraveling.gameObject.SetActive(false);
        vcamGhost.gameObject.SetActive(false);
        uiEndPanel.gameObject.SetActive(false);

        StartCoroutine(__StartCountdown());
    }


    private void Update()
    {
        // If Ghost is playing
        if (ghost.State == GhostState.Playing)
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
        if (ghost.State != GhostState.Playing) return;

        score += amount;
        uiHud.UpdateScore(score);
    }


    public void UpdateHealth(float currHealth, float totalHealth)
    {
        uiHud.UpdateHealth(currHealth, totalHealth);
    }


    public void ShowMessage(string msg)
    {
        uiMsgCanvas.gameObject.SetActive(msg != "");
        txtMsg.text = msg; // FUTURE: do it line by line
    }


    public void FlashMessage(string msg)
    {
        StartCoroutine(__FlashMessage(msg));
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

    private IEnumerator __StartCountdown()
    {
        /* DEBUG: Fast version
        yield return __EndHumanPhase();
        yield break;
        // */

        StartCoroutine(GameManager.I.PlayMusic("ALARM"));
        yield return new WaitForSeconds(3f);
        ShowMessage("WARNING! Global warming has just reached 4 degrees.");
        yield return new WaitForSeconds(5f);
        ShowMessage("The Earth core has become irreversible unstable...");
        yield return new WaitForSeconds(5f);
        ShowMessage("... and unfortunately the planet will explode in...");
        yield return new WaitForSeconds(5f);
        for (int i = secondsToEndOfHumanity; i >= 0; i--)
        {
            ShowMessage($"... {i} ...");
            yield return new WaitForSeconds(1f);
        }
        ShowMessage("KA-BOOOM!!!!");
        GameManager.I.PlaySfx("WORLD_DESTRUCTION");
        StartCoroutine(__TriggerExplosionVfx());
        yield return __EndHumanPhase();
    }
    
    
    private IEnumerator __EndHumanPhase()
    {
        // Place first tile
        Vector3 tilePosition = player.transform.position + Vector3.up * 49f + Vector3.forward * 8f; // FUTURE: this offset shouldn't be magic numbers
        Instantiate(GetRandomTilePrefab(), tilePosition, Quaternion.identity);
        Instantiate(GetRandomTilePrefab(), tilePosition + Vector3.forward * tileLength, Quaternion.identity);

        // Replace model
        player.gameObject.SetActive(false);
        ghostCarrierTr.position = player.transform.position + Vector3.back * ghost.transform.position.z;
        ghost.gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);
        ShowMessage("");
        StartCoroutine(GameManager.I.PlayMusic("TRANSITION"));

        // Travelling camera
        ghost.State = GhostState.Transitioning;
        vcamHuman.gameObject.SetActive(false);
        vcamTraveling.gameObject.SetActive(true);

        yield return new WaitForSeconds(4f);
        ShowMessage("Wait, what? What the @#€∞$???");
        yield return new WaitForSeconds(3f);
        ShowMessage("What happened there? I didn't stand a chance at all!");
        yield return new WaitForSeconds(4f);
        ShowMessage("What kind of stupid game is this!?!");
        yield return new WaitForSeconds(3f);
        ShowMessage("Sigh... so I guess I'm dead, and it's time to rest...");
        yield return new WaitForSeconds(4f);
        ShowMessage("");
        yield return new WaitForSeconds(2f);
        ShowMessage("... or is it?");

        // Ghost camera
        vcamTraveling.gameObject.SetActive(false);
        vcamGhost.gameObject.SetActive(true);
        yield return new WaitForSeconds(4f);
        ShowMessage("");
        StartCoroutine(GameManager.I.PlayMusic("GHOST"));
        ghost.GetComponentInChildren<Light>().gameObject.SetActive(false);

        // Start Ghost phase
        uiHud.gameObject.SetActive(true); // FUTURE: fade in
        ghost.State = GhostState.Playing;

        yield return new WaitForSeconds(8f);
        ShowMessage("This is frustrating, I was really looking forward to baking some muffins");
        yield return new WaitForSeconds(5f);
        ShowMessage("");
        yield return new WaitForSeconds(4f);
        ShowMessage("But apparently I now have to kill all these weird thingies instead");
        yield return new WaitForSeconds(5f);
        ShowMessage("");
        yield return new WaitForSeconds(4f);
        ShowMessage("Which sucks, because they're kind of cute.");
        yield return new WaitForSeconds(5f);
        ShowMessage("");
    }


    private IEnumerator __FlashMessage(string msg)
    {
        if (txtMsg.text == "")
        {
            ShowMessage(msg);
            yield return new WaitForSeconds(4f);
            ShowMessage("");
        }
    }


    private IEnumerator __TriggerExplosionVfx()
    {
        Vector3 center = player.transform.position;
        Instantiate(vfxExplosion, center, Quaternion.identity);
        for (int i = 0; i < 40; i++)
        {
            yield return new WaitForSeconds(0.1f);
            Vector3 randomPosition = center + Vector3.forward * Random.Range(-6f, 6f) + Vector3.left * Random.Range(-6f, 6f);
            Instantiate(vfxExplosion, randomPosition, Quaternion.identity);
        }
    }


    #endregion


    #region Action handlers
    
    private void ProcessOnHealthChange(float currHealth, float totalHealth)
    {
        uiHud.UpdateHealth(currHealth, totalHealth);
    }

    #endregion
}
