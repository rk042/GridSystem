using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    private enum State
    {
        Aiming,
        Shooting,
        Cooloff
    }
    [SerializeField] int maxShootDistance=7;
    private State state;
    private float totalSpin;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShootBool;
    private float shootSpeed=10f;
    [SerializeField] private LayerMask obstacleLayerMask;

    public event EventHandler<ShootActionArg> OnShootTrigger;

    public class ShootActionArg : EventArgs
    {
        public Unit targetUnit,shootUnit;
    }
    void Update()
    {
        if (!isActive)
        {
            return;
        }
        
        stateTimer-=Time.deltaTime;
        switch (state)
        {
            
            case State.Aiming:
                var aimDirection=(targetUnit.GetWorldPosition()-unit.GetWorldPosition()).normalized;
                transform.forward=Vector3.Lerp(transform.forward,aimDirection,Time.deltaTime*shootSpeed);
                break;
            case State.Shooting:
                if (canShootBool)
                {
                    Shoot();
                    canShootBool=false;
                }
                break;
            case State.Cooloff:
                break;
        }
        if (stateTimer<=0)
        {
            NextState();
        }
    }

    private void Shoot()
    {
        OnShootTrigger?.Invoke(this,new ShootActionArg
        {
            targetUnit=targetUnit,
            shootUnit=unit
        });
        targetUnit.Damage(10);
    }

    private void NextState()
    {
        switch (state)
        {
            
            case State.Aiming:                
                state=State.Shooting;
                stateTimer=0.1f;                
                break;
            case State.Shooting:                
                state=State.Cooloff;
                stateTimer=0.5f;                
                break;
            case State.Cooloff:            
                ActionCompleted();                
                break;
        }
    }

    public override string GetActionName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        var unitGridPosition=unit.GetGirdPosition;
        return GetValidActionGridPositionList(unitGridPosition);
    }
    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        var gridPositionList=new List<GridPosition>();

        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                var offsetGridPosition=new GridPosition(x,z);
                var testGridPosition=unitGridPosition+offsetGridPosition;
                
                if (!LevelGrid.instance.IsVelidGridPosition(testGridPosition))
                {
                    continue;
                }
                int testDistnace=Mathf.Abs(x)+Mathf.Abs(z);
                if (testDistnace>maxShootDistance)
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
                Vector3 unitWorldPosition=LevelGrid.instance.GetWorldPosition(unitGridPosition);
                Vector3 shootDir=(targetUnit.GetWorldPosition()-unitWorldPosition).normalized;
                var unitShouldHeight=1.7f;
                var rayHit=Physics.Raycast(unitWorldPosition+Vector3.up*unitShouldHeight,shootDir,Vector3.Distance(unitWorldPosition,targetUnit.GetWorldPosition()),
                    obstacleLayerMask);
                if (rayHit)
                {
                    continue;
                }
                gridPositionList.Add(testGridPosition);
            }
        }

        return gridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit=LevelGrid.instance.GetUnitAtGridPosition(gridPosition);
        canShootBool=true;
        state=State.Aiming;
        stateTimer=2f;
        ActionStart(onActionComplete);
    }

    public Unit GetTargetUnit()
    {
        return targetUnit;
    }

    public int GetShootMaxRange()
    {
        return maxShootDistance;
    }
    protected override EnemyAIAction GetEnemyAIAction(GridPosition item)
    {
        var targetUnit = LevelGrid.instance.GetUnitAtGridPosition(item);

        return new EnemyAIAction
        {
            gridPosition=item,
            actionValue=100 + Mathf.RoundToInt((1-targetUnit.GetHealthNormalize() * 100 ))
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList(gridPosition).Count;
    }

}
