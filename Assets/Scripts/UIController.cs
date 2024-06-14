using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private GameObject mainPanel;

    [Header("InGame")]
    [SerializeField] private TextMeshProUGUI textCurrentScore;

    public void GameStart()
    {
        mainPanel.SetActive(false);
        textCurrentScore.gameObject.SetActive(true);
    }

    public void UpdateScore(int score)
    {
        textCurrentScore.text = score.ToString();
    }
}
