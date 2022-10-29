using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{
    private void Start()
    {
        UnitActionSystem.Instance.OnBusyChange+=OnBusyChange;
        gameObject.SetActive(false);
    }

    private void OnBusyChange(object sender, bool e)
    {
        gameObject.SetActive(e);
    }
}
