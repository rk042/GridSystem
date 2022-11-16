using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public static PathFinding instance;
    private const int Move_Straight_Cost=10;
    private const int Move_Diagonal_Cost=14;
    
    private int width,height;
    private float cellSize;
    private GridSystem<PathNode> gridSystem;
    private void Awake()
    {
        if (instance==null)
        {
            instance=this;
        }
        
        gridSystem = new GridSystem<PathNode>(10,10,2f,
            (GridSystem<PathNode> f,GridPosition gridPosition)=>
            {
                return new PathNode(gridPosition);
            });
    }

    public List<GridPosition> FindPath(GridPosition startPosition,GridPosition endPosition)
    {
        var openList=new List<PathNode>();
        var closeList=new List<PathNode>();

        var startNode=gridSystem.GetGridObject(startPosition);
        var endNode=gridSystem.GetGridObject(endPosition);
        openList.Add(startNode);

        //inti all pathnodes to gridsystem
        for (int x = 0; x < gridSystem.GetWidth; x++)
        {
            for (int z = 0; z < gridSystem.GetHeight; z++)
            {
                var gridPosition=new GridPosition(x,z);
                var pathNode=gridSystem.GetGridObject(gridPosition);

                pathNode.SetGCost(int.MinValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetCameFromPathNode();
            }
        }
        
        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startPosition,endPosition));
        startNode.CalculateFCost();

        while(openList.Count>0)
        {
            PathNode currenNode=GetLowestFCostPathNode(openList);

            if (currenNode==endNode)
            {
                //reached to final node
                return CalculatePath(endNode);
            }

            openList.Remove(currenNode);
            closeList.Add(currenNode);

            foreach (var item in GetNeighbourList(currenNode))
            {
                if (closeList.Contains(item))
                {
                    continue;
                }

                int tentativeCost=currenNode.GetGCost()+CalculateDistance(currenNode.GetCurrentPosition(),item.GetCurrentPosition());

                if (tentativeCost<item.GetFCost())
                {
                    item.SetCameFromPathNode(currenNode);
                    item.SetGCost(tentativeCost);
                    item.SetHCost(CalculateDistance(item.GetCurrentPosition(),endPosition));
                    item.CalculateFCost();

                    if (!openList.Contains(item))
                    {
                        openList.Add(item);
                    }
                }
            }

        }
        // no path found
        return null;
    }

    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodesList=new();
        pathNodesList.Add(endNode);

        PathNode currentNode=endNode;
        while (currentNode.GetCameFromPathNode()!=null)
        {
            pathNodesList.Add(currentNode.GetCameFromPathNode());
            currentNode=currentNode.GetCameFromPathNode();
        }

        pathNodesList.Reverse();

        List<GridPosition> gridPositionsList=new();
        foreach (var item in pathNodesList)
        {
            gridPositionsList.Add(item.GetCurrentPosition());
        }

        return gridPositionsList;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        var neighbourList=new List<PathNode>();
        var gridPosition=currentNode.GetCurrentPosition();

        if (gridPosition.x - 1 >= 0)
        {            
            //Left Node
            neighbourList.Add(GetNode(gridPosition.x-1,gridPosition.z));
            
            if (gridPosition.z-1 >=0)
            {
                //Left Down
                neighbourList.Add(GetNode(gridPosition.x-1,gridPosition.z-1));
            }
            if (gridPosition.z + 1 < gridSystem.GetHeight)
            {                
                //Left UP
                neighbourList.Add(GetNode(gridPosition.x-1,gridPosition.z+1));
            }
        }
        if (gridPosition.x + 1 < gridSystem.GetWidth)
        {            
            //right Node
            neighbourList.Add(GetNode(gridPosition.x+1,gridPosition.z));
            
            if (gridPosition.z-1 >=0)
            {
                //right Down
                neighbourList.Add(GetNode(gridPosition.x+1,gridPosition.z-1));
            }
            if (gridPosition.z + 1 < gridSystem.GetHeight)
            { 
                //right UP
                neighbourList.Add(GetNode(gridPosition.x+1,gridPosition.z+1));
            }
        }

        if (gridPosition.z-1 >=0)
        {
            //down Node
            neighbourList.Add(GetNode(gridPosition.x,gridPosition.z-1));
        }
        if (gridPosition.z + 1 < gridSystem.GetHeight)
        {
            //up Node
            neighbourList.Add(GetNode(gridPosition.x,gridPosition.z+1));
        }

        return neighbourList;
    }

    private PathNode GetNode(int x, int z)
    {
        return gridSystem.GetGridObject(new GridPosition(x,z));
    }

    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodesList)
    {
        PathNode lowestFCostPathNode=pathNodesList[0];
        
        int count = pathNodesList.Count;        
        for (int i = 0; i < count; i++)
        {
            if (pathNodesList[i].GetFCost()<lowestFCostPathNode.GetFCost())
            {
                lowestFCostPathNode=pathNodesList[i];
            }
        }

        return lowestFCostPathNode;
    }

    public int CalculateDistance(GridPosition gridPositionA,GridPosition gridPositionB)
    {
        var gridPositionDistance=gridPositionA-gridPositionB;
        var distance=Mathf.Abs(gridPositionDistance.x) + Mathf.Abs(gridPositionDistance.z);
        int xDistance=Mathf.Abs(gridPositionA.x);
        int zDistance=Mathf.Abs(gridPositionA.z);
        int remaining=Mathf.Abs(xDistance-zDistance);
        return Move_Straight_Cost * Mathf.Min(xDistance,zDistance) + Move_Straight_Cost * remaining;
    }
}
