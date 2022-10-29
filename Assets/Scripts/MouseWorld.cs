using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    private static MouseWorld instance;
    
    private void Awake()
    {
        instance=this;
    }
    
    public static Vector3 GetPosition()
    {
        var ray=Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray,out RaycastHit hit,float.MaxValue,instance.layerMask);
        return hit.point;        
    }
}
