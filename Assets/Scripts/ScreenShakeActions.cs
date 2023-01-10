using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeActions : MonoBehaviour
{
    private void OnEnable()
    {
        ShootAction.OnAnyActionShoot+=OnAnyActionShoot;
        GranadeProjectTile.OnAnyGranadeExploded+=OnAnyGranadeExploded;
    }

    private void OnAnyGranadeExploded(object sender, EventArgs e)
    {
        ScreenShake.instance.Shake(2f);
    }

    private void OnDisable()
    {
        ShootAction.OnAnyActionShoot-=OnAnyActionShoot;
        GranadeProjectTile.OnAnyGranadeExploded-=OnAnyGranadeExploded;
    }

    private void OnAnyActionShoot(object sender, ShootAction.ShootActionArg e)
    {
       ScreenShake.instance.Shake();
    }
}
