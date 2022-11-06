using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnirRagDollSpawner : MonoBehaviour
{
    [SerializeField] Transform ragDollPregab;
    [SerializeField] Transform orignalRagdollRoot;
    private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem=GetComponent<HealthSystem>();
    }

    void Start()
    {
        healthSystem.OnDie+=OnDie;
    }

    private void OnDie(object sender, EventArgs e)
    {
        var ragdoll = Instantiate(ragDollPregab,transform.position,transform.rotation);
        var ragdollTemp = ragdoll.GetComponent<UnitRagdoll>();
        ragdollTemp.SetUp(orignalRagdollRoot);
    }
}
