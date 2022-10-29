using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmpText;
    [SerializeField] Button btnButton;
    [SerializeField] Image imgUpdate;
    private BaseAction baseAction;

    public void SetBaseAction(BaseAction baseAction)
    {
        this.baseAction=baseAction;
        tmpText.text=baseAction.GetActionName().ToUpper();
        btnButton.onClick.AddListener(()=>{
            UnitActionSystem.Instance.SetSelectedAction(baseAction);
        });
    }
    public void UpdateSelectedVisual()
    {
        var selectedBaseAction=UnitActionSystem.Instance.GetSelectedAction;
        imgUpdate.enabled=(selectedBaseAction==baseAction);
    }
}
