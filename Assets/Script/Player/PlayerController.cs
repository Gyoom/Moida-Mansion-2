using System;
using UnityEngine;

public class PlayerController : Actor
{
    public static PlayerController instance;
    
    [SerializeField] private Vector2 playerPos = Vector2.one;
    [SerializeField] private bool isGoingUp = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }

    public override void MoveRight()
    {
        if(playerPos.x >= 4) return;
        Debug.Log($"Move Right !");
        playerPos += Vector2.right;
        
        //TODO : Draw the room you at, update position on array 
    }

    public override void MoveLeft()
    {
        if(playerPos.x <= 0) return;
        Debug.Log($"MoveLeft !");
        playerPos += -Vector2.right;
    }

    public override void Search()
    {
        Debug.Log($"Search !");
    }

    public override void TakeStair()
    {
        if (playerPos.y is <= 0 or >= 3) return;

        Debug.Log($"TakeStair !");
        playerPos += isGoingUp ? Vector2.up : -Vector2.up;
    }
}
