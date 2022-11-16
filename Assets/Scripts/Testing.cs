using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
   /// <summary>
   /// Update is called every frame, if the MonoBehaviour is enabled.
   /// </summary>
   private void Update()
   {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GridPosition mouseGridPosition=LevelGrid.instance.GetGridPosition(MouseWorld.GetPosition());
            GridPosition startGridPosition=new GridPosition(0,0);

            
            var gridPositionList=PathFinding.instance.FindPath(startGridPosition,mouseGridPosition);

            if (gridPositionList==null)
            {
                Debug.Log($"null");
                return;
            }
            int count = gridPositionList.Count;
            for (int i = 0; i < count; i++)
            {
                Debug.DrawLine
                (
                  LevelGrid.instance.GetWorldPosition(gridPositionList[i]),
                  LevelGrid.instance.GetWorldPosition(gridPositionList[i+1]),
                  Color.red,
                  10f
                );
            }
        }
   } 
}
