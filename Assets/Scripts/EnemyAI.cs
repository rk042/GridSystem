using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy,
    }
    private State state;
    private float timer;
    private void Awake()
    {
        state=State.WaitingForEnemyTurn;
    }
    private void Start()
    {
        TurnSystem.instance.OnTurnChanged+=OnTurnChanged;
    }

    private void OnTurnChanged(object sender, EventArgs e)
    {
        if (!TurnSystem.instance.IsPlayerTurn())
        { 
            state=State.TakingTurn;
            timer=2f;
        }
    }

    private void Update()
    {
        if (TurnSystem.instance.IsPlayerTurn())
        {
            return;
        }

        switch (state)
        {
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:
                timer-=Time.deltaTime;
                if (timer<=0f)
                {
                    if(TryTakeEnemyAIAction(SetStateTakingTurn))
                    {
                        state=State.Busy;
                    }
                    else
                    {                        
                        TurnSystem.instance.NextTurn();
                    }
                }
                break;
            case State.Busy:
                break;
        }        
    }

    private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
    {
        foreach (var item in UnitManager.instance.enemyUnitList)
        {
            if (TryTakeEnemyAIAction(item,onEnemyAIActionComplete))
            {  
                return true;
            } 
        }

        return false;
    }

    private bool TryTakeEnemyAIAction(Unit item, Action onEnemyAIActionComplete)
    {
        EnemyAIAction bestEnemyAIAction=null;
        BaseAction bestBaseAIAction=null;
        foreach (var item1 in item.GetbaseActionArray)
        {
            if (!item.CanSpendActionPointsToTakeAction(item1))
            {
                continue;
            }
            if (bestEnemyAIAction==null)
            {
                bestEnemyAIAction=item1.GetBestEnemyAIAction();
                bestBaseAIAction=item1;
            }
            else
            {
                EnemyAIAction testEnemyAIAction=item1.GetBestEnemyAIAction();
                if (testEnemyAIAction!=null && testEnemyAIAction.actionValue > bestEnemyAIAction.actionValue)
                {
                    bestEnemyAIAction=testEnemyAIAction;
                    bestBaseAIAction=item1;
                }
            }
            
        }

        if (bestEnemyAIAction != null && item.TrySpendActionPointsToTakeAction(bestBaseAIAction))
        {
            bestBaseAIAction.TakeAction(bestEnemyAIAction.gridPosition,onEnemyAIActionComplete);
            return true;
        }
        else
        {
            return false;
        }       
    }

    private void SetStateTakingTurn()
    {
        timer=0.5f;
        state=State.TakingTurn;
    }
}
