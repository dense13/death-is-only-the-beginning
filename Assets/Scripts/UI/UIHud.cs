using UnityEngine;
using TMPro;

public class UIHud : MonoBehaviour
{
    [SerializeField] private TMP_Text txtScore;

    #region Public

    public void UpdateScore(int score)
    {
        txtScore.text = string.Format("{0:#.}", score); // FUTURE: update the score in increments of one
    }
        
    #endregion
}
