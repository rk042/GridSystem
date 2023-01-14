using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwardAction : BaseAction
{
    enum State
    {
        SwingingSwordBeforeHit,
        SwingingSwordAfterHit,
    }

    private int maxSwardDistance=1;
    private State state;
    private float stateTimer;
    private Unit targetUnit;

    public int GetShootMaxRange()
    {
        return maxSwardDistance;
    }
    public override bool Equals(object other)
    {
        return base.Equals(other);
    }

    public override string GetActionName()
    {
        return "Sward";
    }

    public override int GetActionPointsCost()
    {
        return base.GetActionPointsCost();
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
         var gridPositionList=new List<GridPosition>();
        GridPosition unitGridPosition = default;

        for (int x = -maxSwardDistance; x <= maxSwardDistance; x++)
        {
            for (int z = -maxSwardDistance; z <= maxSwardDistance; z++)
            {
                var offsetGridPosition=new GridPosition(x,z);
                var testGridPosition=unitGridPosition+offsetGridPosition;
                
                if (!LevelGrid.instance.IsVelidGridPosition(testGridPosition))
                {
                    continue;
                }
                gridPositionList.Add(testGridPosition);
            }
        }

        return gridPositionList;
    }

    public override bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        return base.IsValidActionGridPosition(gridPosition);
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        Debug.Log($"sward Action");
        targetUnit=LevelGrid.instance.GetUnitAtGridPosition(gridPosition);
        state=State.SwingingSwordAfterHit;  
        float afterHitStateTime=0.7f;
        stateTimer=afterHitStateTime;      
        ActionStart(onActionComplete);
    }

    public override string ToString()
    {
        return base.ToString();
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override EnemyAIAction GetEnemyAIAction(GridPosition item)
    {
        return new EnemyAIAction
        {
            gridPosition=item,
            actionValue=200
        };
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
        stateTimer-=Time.deltaTime;
        switch (state)
        {
            case State.SwingingSwordAfterHit:
                break;
            case State.SwingingSwordBeforeHit:
                var aimDirection=(targetUnit.GetWorldPosition()-unit.GetWorldPosition()).normalized;
                float shootSpeed = 10;
                transform.forward=Vector3.Lerp(transform.forward,aimDirection,Time.deltaTime*shootSpeed);
                break;
        }
        if (stateTimer<=0)
        {
            NextState();
        }
    }
    private void NextState()
    {
        switch (state)
        {
            case State.SwingingSwordAfterHit:                
                ActionCompleted();
                break;
            case State.SwingingSwordBeforeHit:       
                state=State.SwingingSwordAfterHit;  
                float afterHitStateTime=0.5f;
                stateTimer=afterHitStateTime;   
                targetUnit.Damage(100);   
                break;
        }
    }
}
