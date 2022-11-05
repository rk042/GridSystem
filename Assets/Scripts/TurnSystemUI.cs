using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] Button btnEntTurn;
    [SerializeField] TextMeshProUGUI turnNumberText;
    [SerializeField] GameObject enemyTurnVisual;

    private void Start()
    {
        btnEntTurn.onClick.AddListener(()=> 
        {
            TurnSystem.instance.NextTurn();
        });
        TurnSystem.instance.OnTurnChanged+=OnTurnChanged;
        UpdateTurnText();
        UpdateEndTurnVisibility();
    }

    private void OnTurnChanged(object sender, EventArgs e)
    {
        UpdateTurnText();
        UpdateEneimeTurnVisual();
        UpdateEndTurnVisibility();
    }

    private void UpdateTurnText()
    {
        turnNumberText.text=$"TURN : {TurnSystem.instance.GetTurnNumber}";
    }

    private void UpdateEneimeTurnVisual()
    {
        enemyTurnVisual.SetActive(!TurnSystem.instance.IsPlayerTurn());
    }

    private void UpdateEndTurnVisibility()
    {
        btnEntTurn.gameObject.SetActive(TurnSystem.instance.IsPlayerTurn());
    }
}
