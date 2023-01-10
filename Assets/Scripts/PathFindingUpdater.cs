using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingUpdater : MonoBehaviour
{
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        DistractableCrat.OnAnyDestoryed+=OnAnyDestory;
    }
    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    private void OnDestroy()
    {
        DistractableCrat.OnAnyDestoryed-=OnAnyDestory;
    }

    private void OnAnyDestory(object sender, EventArgs e)
    {
        var DistractableCrat=sender as DistractableCrat;
        PathFinding.Instance.SetWalkableGridPosition(DistractableCrat.gridPosition,true);
    }
}
