using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager instance{get; private set;}

    public List<Unit> unitList{get; private set;} 
    public List<Unit> friendlyUnitList{get; private set;}
    public List<Unit> enemyUnitList{get; private set;}

    private void Awake()
    {
        instance=this;
        unitList=new();
        friendlyUnitList=new();
        enemyUnitList=new();
    }
    private void Start()
    {
        Unit.OnAnyUnitSpawned+=OnAnyUnitSpawned;
        Unit.OnAnyUnitDeath+=OnAnyUnitDeath;
    }

    private void OnAnyUnitDeath(object sender, EventArgs e)
    {
        Unit unit=sender as Unit;
        unitList.Remove(unit);
        if (unit.IsEnemy())
        {
            enemyUnitList.Remove(unit);
        }
        else
        {
            friendlyUnitList.Remove(unit);
        }
    }

    private void OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit=sender as Unit;
        unitList.Add(unit);
        if (unit.IsEnemy())
        {
            enemyUnitList.Add(unit);
        }
        else
        {
            friendlyUnitList.Add(unit);
        }
    }
}
