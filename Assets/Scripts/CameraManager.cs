using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject actionCameraGameObject;
    
    
    private void Start()
    {
        BaseAction.OnAnyActionStart+=OnAnyActionStarted;    
        BaseAction.OnAnyActionCompleted+=OnAnyActionCompleted;    

        HideActionCamera();
    }

    private void OnAnyActionCompleted(object sender, EventArgs e)
    {
        Debug.Log($"action completed {sender.GetType()}");
        switch (sender)
        {
            case ShootAction shootAction:
                HideActionCamera();
                break;
          
        }
    }

    private void OnAnyActionStarted(object sender, EventArgs e)
    {
        Debug.Log($"action started {sender.GetType()}");
        switch (sender)
        {
            case ShootAction shootAction:   
                var shootUnit=shootAction.GetUnit();
                var targetUnit=shootAction.GetTargetUnit();
                var shootDir=(targetUnit.GetWorldPosition()-shootUnit.GetWorldPosition()).normalized;
                var shoulderOffset=Quaternion.Euler(0,90,0)*shootDir*1.5f;
                var cameraCharacterHeight=Vector3.up*1.7f;
                var actionCameraPosition = shootUnit.GetWorldPosition()+cameraCharacterHeight+shoulderOffset+(shootDir*-1);
                actionCameraGameObject.transform.position=actionCameraPosition;
                actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition()+cameraCharacterHeight);
                
                ShowActionCamera();
                break;
          
        }   
    }

    private void ShowActionCamera()
    {
        actionCameraGameObject.SetActive(true);
        Debug.Log($"show "+actionCameraGameObject.activeInHierarchy);
    }

    private void HideActionCamera()
    {
        actionCameraGameObject.SetActive(false);
        Debug.Log($"hide "+actionCameraGameObject.activeInHierarchy);
    }
}
