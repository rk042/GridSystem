using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmpText;
    [SerializeField] Button btnButton;

    public void SetBaseAction(BaseAction baseAction)
    {
        tmpText.text=baseAction.GetActionName().ToUpper();
        btnButton.onClick.AddListener(()=>{
            UnitActionSystem.Instance.SetSelectedAction(baseAction);
        });
    }
}
