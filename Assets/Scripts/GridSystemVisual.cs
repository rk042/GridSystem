using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual instance {get;private set;}
    [SerializeField] Transform gridSystemVisualSinglePrefab;
    GridSystemVisualSingle[,] gridSystemVisualSinglesArray;
    
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
    }
    private void Update()
    {
        UpdateGridVisual();
    }

    private void UpdateGridVisual()
    {
        HideAllGridPosition();
        ShowGridPositionList(UnitActionSystem.Instance.GetSelectedAction.GetValidActionGridPositionList());
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

    public void ShowGridPositionList(List<GridPosition> gridPositionsList)
    {
        for (int g = 0; g < gridPositionsList.Count; g++)
        {
            gridSystemVisualSinglesArray[gridPositionsList[g].x,gridPositionsList[g].z].Show();
        }
    }
}
