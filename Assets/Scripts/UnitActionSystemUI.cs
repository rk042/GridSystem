using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
 
public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] Transform actionButtonPrefab,actionButtonParent;
    [SerializeField] TextMeshProUGUI actionPointText; 
    private List<ActionButtonUI> actionButtonUIList;

    void Start()
    {
        actionButtonUIList=new List<ActionButtonUI>();
        UnitActionSystem.Instance.OnSelectUnitChange+=OnSelectedUnitChange;
        UnitActionSystem.Instance.OnSelectedActionChange+=OnSelectedActionChange;
        UnitActionSystem.Instance.OnActionStarted+=OnActionStarted;
        TurnSystem.instance.OnTurnChanged+=OnTurnChanged;
        Unit.OnAnyActionPointsChanged+=OnAnyActionPointsChanged;

        CreateUIActionButton();   
        UpdateSelectedVisual();
        UpdateActionPoint();
    }

    private void OnAnyActionPointsChanged(object sender, EventArgs e)
    {
       UpdateActionPoint();
    }

    private void OnTurnChanged(object sender, EventArgs e)
    {
       UpdateActionPoint();
    }

    private void OnActionStarted(object sender, EventArgs e)
    {
        UpdateActionPoint();
    }

    private void OnSelectedActionChange(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
    }

    private void OnSelectedUnitChange(object sender, EventArgs e)
    {     
        CreateUIActionButton();
        UpdateSelectedVisual();
        UpdateActionPoint();
    }

    private void CreateUIActionButton()
    {
        var selectedUnit=UnitActionSystem.Instance.GetSelectUnit();
        foreach (Transform item in actionButtonParent)
        {
            Destroy(item.gameObject);
        }
        actionButtonUIList.Clear();
        foreach (var item in selectedUnit.GetbaseActionArray)
        {
           var buttonItem = Instantiate(actionButtonPrefab,actionButtonParent);
           var actionButtonUI= buttonItem.GetComponent<ActionButtonUI>();
           actionButtonUI.SetBaseAction(item);
           actionButtonUIList.Add(actionButtonUI);
        }
    }
    private void UpdateSelectedVisual()
    {
        foreach (var item in actionButtonUIList)
        {
            item.UpdateSelectedVisual();
        }
    }

    private void UpdateActionPoint()
    {
        var selectedUnit=UnitActionSystem.Instance.GetSelectUnit();
        actionPointText.text=$"Action Point : {selectedUnit.actionPoint}";
    }
}
