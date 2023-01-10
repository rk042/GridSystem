using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{

   /// <summary>
   /// Start is called on the frame when a script is enabled just before
   /// any of the Update methods is called the first time.
   /// </summary>
   private void Start()
   {
         // for (int i = 0; i < 10; i++)
         // {
         //    for (int j = 0; j < 10; j++)
         //    {
         //       if (i<5)
         //       {
         //          // goto isGraterThen5;
         //          Debug.Log($"is grater then 5");
                  
         //       }
         //    } 
         // }

         // isGraterThen5: Debug.Log($"is grater then 5");
   }

   /// <summary>
   /// Update is called every frame, if the MonoBehaviour is enabled.
   /// </summary>
   private void Update()
   {
        // if (Input.GetKeyDown(KeyCode.T))
        // {
        //     GridPosition mouseGridPosition=LevelGrid.instance.GetGridPosition(MouseWorld.GetPosition());
        //     GridPosition startGridPosition=new GridPosition(0,0);

            
        //     var gridPositionList=PathFinding.Instance.FindPath(startGridPosition,mouseGridPosition);

        //     if (gridPositionList==null)
        //     {
        //         Debug.Log($"null");
        //         return;
        //     }
        //     int count = gridPositionList.Count;
        //     for (int i = 0; i < count-1; i++)
        //     {
        //         Debug.DrawLine
        //         (
        //           LevelGrid.instance.GetWorldPosition(gridPositionList[i]),
        //           LevelGrid.instance.GetWorldPosition(gridPositionList[i+1]),
        //           Color.red,
        //           10f
        //         );
        //     }
        // }
   } 
}
