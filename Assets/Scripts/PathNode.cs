using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private GridPosition gridPosition;
    private int gCost,fCost,hCost;
    private PathNode cameFromPathNode;
    
    public PathNode(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
    }

    internal void SetGCost(int gCost)
    {
        this.gCost=gCost;
    }

    internal void SetHCost(int hCost)
    {
        this.hCost=hCost;
    }
    
    internal int GetFCost()
    {
        return fCost;
    }
    internal void CalculateFCost()
    {
        this.fCost=gCost+hCost;
    }

    internal void ResetCameFromPathNode()
    {
        cameFromPathNode=null;
    }

    internal GridPosition GetCurrentPosition()
    {
       return gridPosition;
    }
    internal int GetGCost()
    {
        return gCost;
    }
    internal int GetHCost()
    {
        return hCost;
    }

    internal void SetCameFromPathNode(PathNode currenNode)
    {
        cameFromPathNode=currenNode;
    }

    internal PathNode GetCameFromPathNode()
    {
        return cameFromPathNode;
    }
}
