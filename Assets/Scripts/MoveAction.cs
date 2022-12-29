using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{   
    [SerializeField] int maxMoveDistance=4;
    [SerializeField] private float myRotationSpeed=10;
    private float moveSpeed=4f;
    private List<Vector3> positionList;
    private int currentPositionIndex;

    public Action OnActionCompleted { get; private set; }
    public event EventHandler OnStartMoving,OnStopMoving;
    
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        var pathGridPositionList= PathFinding.Instance.FindPath(unit.GetGirdPosition,gridPosition, out int pathLength);
        currentPositionIndex=0;
        positionList=new List<Vector3>();
        
        foreach (var item in pathGridPositionList)
        {
            positionList.Add(LevelGrid.instance.GetWorldPosition(item));
        }

        OnStartMoving?.Invoke(this,EventArgs.Empty);
        ActionStart(onActionComplete);
    }
    
    private void Update()
    {
        if (!isActive)
        {
            return;
        }
        var targetPosition=positionList[currentPositionIndex];
        var moveDirection = (targetPosition - transform.position).normalized;
        if (Vector3.Distance(transform.position,targetPosition)>.1f)
        {
            transform.position+=moveDirection * Time.deltaTime *moveSpeed; 
        }
        else
        {
            currentPositionIndex++;
            if (currentPositionIndex>=positionList.Count)
            {    
                OnStopMoving?.Invoke(this,EventArgs.Empty);
                ActionCompleted();
            }
        }
        transform.forward=Vector3.Lerp(transform.forward,moveDirection,Time.deltaTime*myRotationSpeed);
    }
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        var gridPositionList=new List<GridPosition>();
        var unitGridPosition=unit.GetGirdPosition;

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                var offsetGridPosition=new GridPosition(x,z);
                var testGridPosition=unitGridPosition+offsetGridPosition;

                if (!LevelGrid.instance.IsVelidGridPosition(testGridPosition))
                {
                    continue;
                }
                if (LevelGrid.instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    continue;
                }
                if (!PathFinding.Instance.IsWalkableGridPosition(testGridPosition))
                {
                    continue;
                }
                if (!PathFinding.Instance.HasPath(unitGridPosition,testGridPosition))
                {
                    continue;
                }
                if (PathFinding.Instance.GetPathLength(unitGridPosition,testGridPosition)>maxMoveDistance * 10)
                {
                    continue;
                }
                gridPositionList.Add(testGridPosition);
            }
        }

        return gridPositionList;
    }

    public override string GetActionName()
    {
        return "MoveAction";
    }

    protected override EnemyAIAction GetEnemyAIAction(GridPosition item)
    {
        var targetCountAtGridPosition = unit.GetAction<ShootAction>().GetTargetCountAtPosition(item);
        return new EnemyAIAction{
            gridPosition=item,
            actionValue=targetCountAtGridPosition*10
        };
    }
}
