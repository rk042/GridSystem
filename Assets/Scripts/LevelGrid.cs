using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid instance{get;private set;}
    public event EventHandler OnAnyUnitMovedGridPosition;

    private GridSystem<GridObject> gridSystem;
    public int GetWidth=>gridSystem.GetWidth;
    public int GetHeight=>gridSystem.GetHeight;
    void Awake()
    {
        instance=this;
        gridSystem=new GridSystem<GridObject>(10,10,2,(GridSystem<GridObject> g,GridPosition gridPosition)=>new GridObject(g,gridPosition));
    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        PathFinding.Instance.Setup(10,10,2);
    }
    public void AddUnitAtGridPosition(GridPosition gridPosition,Unit unit)
    {
        gridSystem.GetGridObject(gridPosition).unitList.Add(unit);
        // Debug.Log($"add => {gridSystem.GetGridObject(gridPosition).unitList.Count}");
    }

    // public List<Unit> GetUnitAtGridPosition(GridPosition gridPosition)
    // {
    //     return gridSystem.GetGridObject(gridPosition).unitList;
    // }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition,Unit unit)
    {
        gridSystem.GetGridObject(gridPosition).unitList.Remove(unit);
        // Debug.Log($"remove => {gridSystem.GetGridObject(gridPosition).unitList.Count}");
    }

    public void UnitMoveGridPosition(Unit unit,GridPosition fromGridPosition,GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition,unit);
        AddUnitAtGridPosition(toGridPosition,unit);

        OnAnyUnitMovedGridPosition?.Invoke(this,EventArgs.Empty);
    }

    public GridPosition GetGridPosition(Vector3 worldPos) => gridSystem.GetGridPosition(worldPos);
    public bool IsVelidGridPosition(GridPosition gridPosition) => gridSystem.IsVelidGridPosition(gridPosition);
    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        return gridSystem.GetGridObject(gridPosition).HasAnyUnit();
    }
    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        return gridSystem.GetGridObject(gridPosition).GetUnit();
    }
    public Vector3 GetWorldPosition(GridPosition gridPosition)=>gridSystem.GetWorldPosition(gridPosition);
}
