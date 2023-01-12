using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeAction : BaseAction
{
    private int maxThrowDistance=7;
    [SerializeField] LayerMask obstacleLayerMask;
    [SerializeField] GranadeProjectTile grenadeProjectilePrefab;
    
    public int GetShootMaxRange()
    {
        return maxThrowDistance;
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        if (!isActive)
        {
            return;
        }

       
    }
    public override string GetActionName()
    {
       return "Granade";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        var gridPositionList=new List<GridPosition>();
        GridPosition unitGridPosition = default;

        for (int x = -maxThrowDistance; x <= maxThrowDistance; x++)
        {
            for (int z = -maxThrowDistance; z <= maxThrowDistance; z++)
            {
                var offsetGridPosition=new GridPosition(x,z);
                var testGridPosition=unitGridPosition+offsetGridPosition;
                
                if (!LevelGrid.instance.IsVelidGridPosition(testGridPosition))
                {
                    continue;
                }
                
                int testDistnace=Mathf.Abs(x)+Mathf.Abs(z);
                if (testDistnace>maxThrowDistance)
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
        var granade=  Instantiate(grenadeProjectilePrefab,unit.GetWorldPosition(),Quaternion.identity);
        granade.Setup(gridPosition,OnGranadeComplete);
        ActionStart(onActionComplete);
    }

    private void OnGranadeComplete()
    {
         ActionCompleted();
    }

    protected override EnemyAIAction GetEnemyAIAction(GridPosition item)
    {
        return new EnemyAIAction()
        {
            gridPosition=item,
            actionValue=0
        };
    }
}
