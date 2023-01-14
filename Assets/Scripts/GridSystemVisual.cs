using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual instance {get;private set;}
    [SerializeField] Transform gridSystemVisualSinglePrefab;
    [SerializeField] List<GridVisualTypeMaterial> gridVisualTypeMaterialsList=new List<GridVisualTypeMaterial>();

    GridSystemVisualSingle[,] gridSystemVisualSinglesArray;
    
    [System.Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType gridVisualType;
        public Material material;
    }

    public enum GridVisualType
    {
        White,
        Blue,
        Red,
        RedSoft,
        Yellow
    }



    private void Awake()
    {
        instance=this;
    }
    private void Start()
    {
        gridSystemVisualSinglesArray=new GridSystemVisualSingle[LevelGrid.instance.GetWidth,LevelGrid.instance.GetHeight];

        for (int x = 0; x < LevelGrid.instance.GetWidth; x++)
        {
            for (int z = 0; z < LevelGrid.instance.GetHeight; z++)
            {
               var myObject= Instantiate(gridSystemVisualSinglePrefab,LevelGrid.instance.GetWorldPosition(new GridPosition(x,z)),Quaternion.identity);
               gridSystemVisualSinglesArray[x,z]=myObject.GetComponent<GridSystemVisualSingle>();
            }
        }    

        UnitActionSystem.Instance.OnSelectedActionChange+=OnSelectedActionChange;
        LevelGrid.instance.OnAnyUnitMovedGridPosition+=OnAnyUnitMovedGridPosition;

        UpdateGridVisual();
    }

    private void ShowGridPositionRange(GridPosition gridPosition,int range,GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionsList =new List<GridPosition>();

        for (int x = -range ; x <= range; x++)
        {
            for (int z = -range ; z <= range; z++)
            {
                var testGridPosition=gridPosition+new GridPosition(x,z);
                int testDistnace=Mathf.Abs(x)+Mathf.Abs(z);
                if (testDistnace>range)
                {
                    continue;
                }
                if (!LevelGrid.instance.IsVelidGridPosition(testGridPosition))
                {
                    continue;
                }
                gridPositionsList.Add(testGridPosition);
            }
        }

        ShowGridPositionList(gridPositionsList,gridVisualType);
    }
    private void ShowGridPositionRangeSquare(GridPosition gridPosition,int range,GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionsList =new List<GridPosition>();

        for (int x = -range ; x <= range; x++)
        {
            for (int z = -range ; z <= range; z++)
            {
                var testGridPosition=gridPosition+new GridPosition(x,z);
        
                if (!LevelGrid.instance.IsVelidGridPosition(testGridPosition))
                {
                    continue;
                }
                gridPositionsList.Add(testGridPosition);
            }
        }

        ShowGridPositionList(gridPositionsList,gridVisualType);
    }
    private void OnAnyUnitMovedGridPosition(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }

    private void OnSelectedActionChange(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }

    private void UpdateGridVisual()
    {
        HideAllGridPosition();
        var selectedUnit = UnitActionSystem.Instance.GetSelectUnit();
        BaseAction getSelectedAction = UnitActionSystem.Instance.GetSelectedAction;
        GridVisualType gridVisualType;
        switch (getSelectedAction)
        {
            default:
            case MoveAction moveAction:
                gridVisualType=GridVisualType.White;
                break;
            case SpinAction spinAction:
                gridVisualType=GridVisualType.Blue;
                break;
            case ShootAction shootAction:
                gridVisualType=GridVisualType.Red;
                ShowGridPositionRange(selectedUnit.GetGirdPosition,shootAction.GetShootMaxRange(),GridVisualType.RedSoft);
                break;            
            case GranadeAction granadeAction:
                gridVisualType=GridVisualType.Yellow;
                ShowGridPositionRange(selectedUnit.GetGirdPosition,granadeAction.GetShootMaxRange(),GridVisualType.RedSoft);
                break;            
            case SwardAction swardAction:
                gridVisualType=GridVisualType.Red;
                ShowGridPositionRange(selectedUnit.GetGirdPosition,swardAction.GetShootMaxRange(),GridVisualType.RedSoft);
                // ShowGridPositionRangeSquare(selectedUnit.GetGirdPosition,swardAction.GetShootMaxRange(),GridVisualType.RedSoft);
                break;
        }

        ShowGridPositionList(getSelectedAction.GetValidActionGridPositionList(),gridVisualType);
    }

    public void HideAllGridPosition()
    {
        for (int x = 0; x < LevelGrid.instance.GetWidth; x++)
        {
            for (int z = 0; z < LevelGrid.instance.GetHeight; z++)
            {
               gridSystemVisualSinglesArray[x,z].Hide();
            }
        } 
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionsList,GridVisualType gridVisualType)
    {
        for (int g = 0; g < gridPositionsList.Count; g++)
        {
            gridSystemVisualSinglesArray[gridPositionsList[g].x,gridPositionsList[g].z].Show(GetGridVisualTypeMaterial(gridVisualType));
        }
    }

    public Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
    {
        foreach (var item in gridVisualTypeMaterialsList)
        {
            if (item.gridVisualType==gridVisualType)
            {
                return item.material;
            }
        }

        return null;
    }
}
