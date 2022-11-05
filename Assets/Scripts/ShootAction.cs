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
        OnShootTrigger?.Invoke(null,new ShootActionArg
        {
            targetUnit=targetUnit,
            shootUnit=unit
        });
        targetUnit.Damage();
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
        var gridPositionList=new List<GridPosition>();
        var unitGridPosition=unit.GetGirdPosition;

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
                gridPositionList.Add(testGridPosition);
            }
        }

        return gridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        ActionStart(onActionComplete);
        targetUnit=LevelGrid.instance.GetUnitAtGridPosition(gridPosition);
        canShootBool=true;
        state=State.Aiming;
        stateTimer=1f;
    }
}
