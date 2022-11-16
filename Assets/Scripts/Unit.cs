using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] bool isEnemie;
    private const int Action_Points_Max=2;
    
    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDeath;

    public GridPosition GetGirdPosition{get;private set;}
    private MoveAction moveAction;
    private ShootAction shootAction;
    private SpinAction spinAction;
    private BaseAction[] baseActionArray;
    public BaseAction[] GetbaseActionArray=>baseActionArray;
    public int actionPoint{get; private set;}
    private HealthSystem healthSystem;
    private void Awake()
    {    
        healthSystem=GetComponent<HealthSystem>();
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
        healthSystem.OnDie+=OnDie;
        OnAnyUnitSpawned?.Invoke(this,EventArgs.Empty);
    }

    private void OnDie(object sender, EventArgs e)
    {
        LevelGrid.instance.RemoveUnitAtGridPosition(GetGirdPosition,this);
        OnAnyUnitDeath?.Invoke(this,EventArgs.Empty);
        Destroy(gameObject);
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

    internal void Damage(int takeDamage)
    {
        healthSystem.Damage(takeDamage);
    }

    private void Update()
    {
        var newGridPosition=LevelGrid.instance.GetGridPosition(transform.position);
        if (newGridPosition!=GetGirdPosition)
        {
            var oldGridPos=GetGirdPosition;
            GetGirdPosition=newGridPosition;
            LevelGrid.instance.UnitMoveGridPosition(this,oldGridPos,newGridPosition);
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
    public float GetHealthNormalize()=>healthSystem.GetHealthNormalized();

    public T GetAction<T>() where T : BaseAction
    {
        foreach (var item in baseActionArray)
        {
            if (item is T)
            {
                return (T)item;
            }
        }

        return null;
    }
}
