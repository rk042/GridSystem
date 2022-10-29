using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public GridPosition gridPosition{get;private set;}
    public MoveAction moveAction{get;private set;}
    public SpinAction spinAction{get;private set;}
    private BaseAction[] baseActionArray;
    public BaseAction[] GetbaseActionArray=>baseActionArray;

    private void Awake()
    {
        moveAction=GetComponent<MoveAction>();
        spinAction=GetComponent<SpinAction>();
        baseActionArray=GetComponents<BaseAction>();
    }
    private void Start()
    {
        gridPosition=LevelGrid.instance.GetGridPosition(transform.position);
        LevelGrid.instance.AddUnitAtGridPosition(gridPosition,this);
    }
    private void Update()
    {
        var newGridPosition=LevelGrid.instance.GetGridPosition(transform.position);
        if (newGridPosition!=gridPosition)
        {
            LevelGrid.instance.UnitMoveGridPosition(this,gridPosition,newGridPosition);
            gridPosition=newGridPosition;
        }
    }
}
