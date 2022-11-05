using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem instance{get;private set;}
    public event System.EventHandler OnTurnChanged;
    private int turnNumber=1;
    public int GetTurnNumber=>turnNumber;
    private bool isPlayerTurn=true;
    private void Awake()
    {
        if (instance==null)
        {
            instance=this;
        }
    }
    
    public void NextTurn()
    {
        turnNumber++;
        isPlayerTurn = !isPlayerTurn;
        OnTurnChanged?.Invoke(this,System.EventArgs.Empty);
    }
    public bool IsPlayerTurn()=>isPlayerTurn;
}
