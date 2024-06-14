using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private GameObject mainPanel;

    public void GameStart()
    {
        mainPanel.SetActive(false);
    }
}
