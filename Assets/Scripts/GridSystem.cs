using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem<TGridObject>
{
    private int width;
    public int GetWidth=>width;
    private int height;
    public int GetHeight=>height;
    private float cellSize;
    private TGridObject[,] gridObjectArray;
    public GridSystem(int wid,int hei,float cellSize,System.Func<GridSystem<TGridObject>,GridPosition,TGridObject> createGridObject)
    {
        width=wid;
        height=hei;
        this.cellSize=cellSize;
        gridObjectArray=new TGridObject[wid,hei];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                gridObjectArray[x,z] = createGridObject(this,new GridPosition(x,z));
                Debug.DrawLine(GetWorldPosition(new GridPosition(x,z)),GetWorldPosition(new GridPosition(x,z)) + Vector3.right * .2f,Color.white,float.MaxValue);
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x,0,gridPosition.z) * cellSize;
    }

    public GridPosition GetGridPosition(Vector3 worldPos)
    {
        return new GridPosition
        (
            Mathf.RoundToInt(worldPos.x/cellSize),
            Mathf.RoundToInt(worldPos.z/cellSize)
        );
    }

    public TGridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjectArray[gridPosition.x,gridPosition.z];
    }

    public bool IsVelidGridPosition(GridPosition gridPosition)
    {
        return  gridPosition.x>=0 &&
                gridPosition.z>=0 &&
                gridPosition.x<width &&
                gridPosition.z<height;
    }
}
