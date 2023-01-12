using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwardAction : BaseAction
{
    private int maxSwardDistance=1;
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
                if (!LevelGrid.instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    continue;
                }
                var targetUnit=LevelGrid.instance.GetUnitAtGridPosition(testGridPosition);
                if (targetUnit.IsEnemy() == unit.IsEnemy())
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

        ActionCompleted();
    }
}
