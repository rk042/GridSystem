using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{   
    [SerializeField] int maxMoveDistance=4;
    [SerializeField] private float myRotationSpeed=10;
    private float moveSpeed=4f;
    private Vector3 targetPostion;

    public Action OnActionCompleted { get; private set; }
    public event EventHandler OnStartMoving,OnStopMoving;
    protected override void Awake()
    {
        base.Awake();
        targetPostion=transform.position;
    }
    
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        this.targetPostion=LevelGrid.instance.GetWorldPosition(gridPosition);
        OnStartMoving?.Invoke(this,EventArgs.Empty);
        ActionStart(onActionComplete);
    }
    
    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        var moveDirection = (targetPostion - transform.position).normalized;
        if (Vector3.Distance(transform.position,targetPostion)>.1f)
        {
            transform.position+=moveDirection * Time.deltaTime *moveSpeed; 
        }
        else
        {
            OnStopMoving?.Invoke(this,EventArgs.Empty);
            ActionCompleted();
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
