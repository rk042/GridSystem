using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDie;
    public event EventHandler OnDamaged;
    [SerializeField] private int health=100;
    private int healthMAX=100;
    
    private void Awake()
    {
        healthMAX=health;
    }
    public void Damage(int takeDamage)
    {
        health-=takeDamage;

        if (health<=0)
        {
            health=0;
        }
        OnDamaged?.Invoke(null,EventArgs.Empty);
        if (health==0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDie?.Invoke(null,EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return (float)health/ healthMAX;
    }
}
