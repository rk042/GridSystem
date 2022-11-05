using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] bool isEnemie;
    private const int Action_Points_Max=2;
    public static event EventHandler OnAnyActionPointsChanged;
    public GridPosition GetGirdPosition{get;private set;}
    public MoveAction moveAction{get;private set;}
    public SpinAction spinAction{get;private set;}
    private BaseAction[] baseActionArray;
    public BaseAction[] GetbaseActionArray=>baseActionArray;
    public int actionPoint{get; private set;}
    private void Awake()
    {
        moveAction=GetComponent<MoveAction>();
        spinAction=GetComponent<SpinAction>();
        baseActionArray=GetComponents<BaseAction>();
        actionPoint=Action_Points_Max;
    }
    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }
    private void Start()
    {
        GetGirdPosition=LevelGrid.instance.GetGridPosition(transform.position);
        LevelGrid.instance.AddUnitAtGridPosition(GetGirdPosition,this);
        TurnSystem.instance.OnTurnChanged+=OnTurnChanged;
    }

    private void OnTurnChanged(object sender, EventArgs e)
    {
        if ((isEnemie && !TurnSystem.instance.IsPlayerTurn()) ||
            (!isEnemie && TurnSystem.instance.IsPlayerTurn()))
        {
            actionPoint=Action_Points_Max;
            OnAnyActionPointsChanged?.Invoke(this,EventArgs.Empty);
        }
    }

    internal void Damage()
    {

    }

    private void Update()
    {
        var newGridPosition=LevelGrid.instance.GetGridPosition(transform.position);
        if (newGridPosition!=GetGirdPosition)
        {
            LevelGrid.instance.UnitMoveGridPosition(this,GetGirdPosition,newGridPosition);
            GetGirdPosition=newGridPosition;
        }
    }
    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if(actionPoint>=baseAction.GetActionPointsCost())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void SpendActionPoints(int amount)
    {
        actionPoint-=amount;
        OnAnyActionPointsChanged?.Invoke(this,EventArgs.Empty);
    }
    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsEnemy()=>isEnemie;
}
