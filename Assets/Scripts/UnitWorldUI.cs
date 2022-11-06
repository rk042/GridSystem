using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI actionPointsText;
    [SerializeField] Unit unit;
    [SerializeField] Image imgHealth;
    [SerializeField] HealthSystem healthSystem;

    private void Start()
    {
        Unit.OnAnyActionPointsChanged+=OnAnyActionPointChanged;
        healthSystem.OnDamaged+=OnDamage;
        UpdateActionPointsText();
        UpdateHealthBar();
    }

    private void OnDamage(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }

    private void OnAnyActionPointChanged(object sender, EventArgs e)
    {
        UpdateActionPointsText();
    }

    private void UpdateActionPointsText()
    {
        actionPointsText.text=unit.actionPoint.ToString();
    }

    private void UpdateHealthBar()
    {
        imgHealth.fillAmount=healthSystem.GetHealthNormalized();
    }
}
