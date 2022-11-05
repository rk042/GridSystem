using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    public System.Action SpinCompleteDelegate;
    private float totalSpin;

    void Update()
    {
        if (!isActive)
        {
            return;
        }

        float y = 360 * Time.deltaTime;
        transform.eulerAngles+=new Vector3(0, y, 0);
        totalSpin+=y;

        if (totalSpin>=360f)
        {
            ActionCompleted();
        }
    }
    public override string GetActionName()
    {
        return "Spin";
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        ActionStart(onActionComplete);
        totalSpin=0;
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        return new List<GridPosition>{unit.GetGirdPosition};
    }

    public override int GetActionPointsCost()
    {
        return 2;
    }
}
