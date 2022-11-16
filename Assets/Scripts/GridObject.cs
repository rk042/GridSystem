using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject 
{
    private GridSystem<GridObject> gridSystem;
    private GridPosition gridPosition;
    public List<Unit> unitList;

    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        unitList=new List<Unit>();
    }

    public bool HasAnyUnit()
    {
        return unitList.Count>0;
    }

    public Unit GetUnit()
    {
        if (HasAnyUnit())
        {
            return unitList[0];
        }
        else
        {
            return null;
        }
    }
}