using System;
using UnityEngine;

public class PlayerController : Actor
{
    public static PlayerController instance;

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
        Debug.Log($"Move Right !");
        
        //TODO : Draw the room you at, update position on array 
    }

    public override void MoveLeft()
    {
        Debug.Log($"MoveLeft !");
    }

    public override void Search()
    {
        Debug.Log($"Search !");
    }

    public override void TakeStair()
    {
        Debug.Log($"TakeStair !");
    }
}
