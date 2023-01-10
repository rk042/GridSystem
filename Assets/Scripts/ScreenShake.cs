using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake instance{get; private set;}

    private CinemachineImpulseSource cinemachineImpulseSource;


    private void Awake()
    {
        if (instance==null)
        {
            instance=this;
        }
        cinemachineImpulseSource=GetComponent<CinemachineImpulseSource>();
    }

    public void Shake(float range=1f)
    {
         cinemachineImpulseSource.GenerateImpulse(range);
    }
}
