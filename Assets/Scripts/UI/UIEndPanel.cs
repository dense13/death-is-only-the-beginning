using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIEndPanel : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject panelTopScoreGO;
    [SerializeField] private GameObject panelNewTopScoreGO;
    [SerializeField] private TMP_Text txtTopScore;
    [SerializeField] private TMP_Text txtNewTopScore;


    private void Awake() {
        panelTopScoreGO.SetActive(true);
        panelNewTopScoreGO.SetActive(false);
        txtTopScore.text = "" + GameManager.I.TopScore;
    }


    public void ShowNewTopScore()
    {
        txtNewTopScore.text = "" + GameManager.I.TopScore;
        panelTopScoreGO.SetActive(false);
        panelNewTopScoreGO.SetActive(true);
    }


    public void BtClickRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void BtClickQuit()
    {
        Application.Quit();
    }
}