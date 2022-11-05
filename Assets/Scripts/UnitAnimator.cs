using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] Animator animator;
    private int isWalking=Animator.StringToHash("IsWalking");
    private int isShoot=Animator.StringToHash("IsShoot");
    [SerializeField] GameObject bulletProjectTrailPrefab;
    [SerializeField] Transform bulletShootStartPoint;
    private void Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += OnStartMoving;
            moveAction.OnStopMoving += OnStopMoving;
        }   
        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShootTrigger+=OnShootTrigger;
        }   
    }

    private void OnShootTrigger(object sender, ShootAction.ShootActionArg e)
    {
        var bullet = Instantiate(bulletProjectTrailPrefab,bulletShootStartPoint.position,Quaternion.identity);
        bullet.GetComponent<BulletProjectTrail>().SetUpPosition(e.targetUnit.GetWorldPosition());
        animator.SetTrigger(isShoot);
    }

    private void OnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool(isWalking,false);
    }

    private void OnStartMoving(object sender, EventArgs e)
    {
        animator.SetBool(isWalking,true);
    }
}
