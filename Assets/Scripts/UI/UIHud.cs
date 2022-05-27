using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHud : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private TMP_Text txtScore;
    [SerializeField] private Image imgCurrHealth;

    #region Public

    public void UpdateScore(int score)
    {
        txtScore.text = string.Format("{0:#.}", score); // FUTURE: update the score in increments of one
    }


    public void UpdateHealth(float currHealth, float totalHealth)
    {
        imgCurrHealth.fillAmount = currHealth / totalHealth; // FUTURE: smooth update
    }
        
    #endregion
}
