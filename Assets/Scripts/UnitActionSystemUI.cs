using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] Transform actionButtonPrefab,actionButtonParent;

    void Start()
    {
        UnitActionSystem.Instance.OnSelectUnitChange+=OnSelectedUnitChange;
        CreateUIActionButton();   
    }

    private void OnSelectedUnitChange(object sender, EventArgs e)
    {     
        CreateUIActionButton();
    }

    private void CreateUIActionButton()
    {
        var selectedUnit=UnitActionSystem.Instance.GetSelectUnit();
        foreach (Transform item in actionButtonParent)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in selectedUnit.GetbaseActionArray)
        {
           var buttonItem = Instantiate(actionButtonPrefab,actionButtonParent);
           buttonItem.GetComponent<ActionButtonUI>().SetBaseAction(item);
        }
    }
}
