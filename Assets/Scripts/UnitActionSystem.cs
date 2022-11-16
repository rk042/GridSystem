using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance{get; private set;}
    public event EventHandler OnSelectUnitChange;
    public event EventHandler OnSelectedActionChange;
    public event EventHandler<bool> OnBusyChange;
    public event EventHandler OnActionStarted;

    [SerializeField] Unit selectedUnit;
    [SerializeField] LayerMask layerMask;
    BaseAction selectedAction;
    public BaseAction GetSelectedAction=>selectedAction;

    private void Awake()
    {
        Instance=this;
    }
    
    private void Start()
    {
        SetSelectedUnit(selectedUnit);
    }
    public bool isBusy=false;
    private void Update()
    {
        if(isBusy)return;
        if(!TurnSystem.instance.IsPlayerTurn()) return;
        if(EventSystem.current.IsPointerOverGameObject()) return;
        if(TryHandleUnitSelection())return;
        HandleSelectedAction();
    }

    private bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {              
            var ray=Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray,out RaycastHit hit,float.MaxValue,layerMask))
            {
                if (hit.transform.TryGetComponent<Unit>(out Unit selectObject))
                {
                    if (selectObject==selectedUnit)
                    {
                        return false;
                    }
                    if (selectObject.IsEnemy())
                    {
                        return false;
                    }

                    SetSelectedUnit(selectObject);
                    return true;
                }
            }
        }

        return false;
    }

    private void SetSelectedUnit(Unit selectObject)
    {
        selectedUnit = selectObject;
        SetSelectedAction(selectObject.GetAction<MoveAction>());
        OnSelectUnitChange?.Invoke(this,EventArgs.Empty);
    }

    public Unit GetSelectUnit()
    {
        return selectedUnit;
    }
    private void Clearbusy()
    {
        isBusy=false;
        OnBusyChange?.Invoke(this,isBusy);
    }
    private void SetBusy()
    {
        isBusy=true;
        OnBusyChange?.Invoke(this,isBusy);
    }
    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction=baseAction;
        OnSelectedActionChange?.Invoke(this,EventArgs.Empty);
    }
    private void HandleSelectedAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mouseGridPosition=LevelGrid.instance.GetGridPosition(MouseWorld.GetPosition());

            if (selectedAction.IsValidActionGridPosition(mouseGridPosition))
            { 
                if(selectedUnit.TrySpendActionPointsToTakeAction(selectedAction))
                {
                    SetBusy();
                    selectedAction.TakeAction(mouseGridPosition,Clearbusy);

                    OnActionStarted?.Invoke(this,EventArgs.Empty);
                }
            }
        }
    }
}
