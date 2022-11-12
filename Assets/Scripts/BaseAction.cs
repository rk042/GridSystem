using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    public static event EventHandler OnAnyActionStart;
    public static event EventHandler OnAnyActionCompleted;

    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;
    protected virtual void Awake()
    {
        unit=GetComponent<Unit>();
    }

    public abstract string GetActionName();
    public abstract void TakeAction(GridPosition gridPosition,System.Action onActionComplete);

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList().Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidActionGridPositionList();

    public virtual int GetActionPointsCost()
    {
        return 1;
    }

    protected void ActionStart(Action onActionComplete)
    {
        isActive=true;
        this.onActionComplete=onActionComplete;
        OnAnyActionStart?.Invoke(this,EventArgs.Empty);
    }

    protected void ActionCompleted()
    {
        isActive=false;
        onActionComplete();
        OnAnyActionCompleted?.Invoke(this,EventArgs.Empty);
    }

    public Unit GetUnit()
    {
        return unit;
    }
}
