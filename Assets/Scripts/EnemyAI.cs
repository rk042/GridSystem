using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private float timer;
    private void Start()
    {
        TurnSystem.instance.OnTurnChanged+=OnTurnChanged;
    }

    private void OnTurnChanged(object sender, EventArgs e)
    {
        timer=2f;
    }

    private void Update()
    {
        if (TurnSystem.instance.IsPlayerTurn())
        {
            return;
        }
        timer-=Time.deltaTime;
        if (timer<=0f)
        {
            TurnSystem.instance.NextTurn();
        }
    }
}
