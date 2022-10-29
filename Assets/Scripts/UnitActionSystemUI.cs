using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] Transform actionButtonPrefab,actionButtonParent;
    private List<ActionButtonUI> actionButtonUIList;

    void Start()
    {
        actionButtonUIList=new List<ActionButtonUI>();
        UnitActionSystem.Instance.OnSelectUnitChange+=OnSelectedUnitChange;
        UnitActionSystem.Instance.OnSelectedActionChange+=OnSelectedActionChange;
        CreateUIActionButton();   
        UpdateSelectedVisual();
    }

    private void OnSelectedActionChange(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
    }

    private void OnSelectedUnitChange(object sender, EventArgs e)
    {     
        CreateUIActionButton();
        UpdateSelectedVisual();
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
}
