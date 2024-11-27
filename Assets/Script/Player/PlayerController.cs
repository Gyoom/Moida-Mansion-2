using System;
using Script.Procedural_Generation;
using UnityEngine;

public class PlayerController : Actor
{
    public static PlayerController instance;
    
    [SerializeField] private Vector2 playerPos = Vector2.one;
    [SerializeField] private bool isGoingUp = false;

    private bool isSearching;
    private float timeToWait;
    private int maxTimeToWait = 2;

    private RoomObj objToSearch;
    
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
        isSearching = false;
        
        if(playerPos.x >= 4) return;
        Debug.Log($"Move Right !");
        playerPos += Vector2.right;
        
        //TODO : Draw the room you at, update position on array 
    }

    public override void MoveLeft()
    {
        isSearching = false;
        
        if(playerPos.x <= 0) return;
        Debug.Log($"MoveLeft !");
        playerPos += -Vector2.right;
    }

    public override void Search()
    {
        Debug.Log($"Search !");
        isSearching = true;
        timeToWait = maxTimeToWait;

        objToSearch = SearchTest.instance.GetAObjToSearch();
        
    }

    public override void TakeStair()
    {
        isSearching = false;
        
        if (playerPos.y is <= 0 or >= 3) return;

        Debug.Log($"TakeStair !");
        playerPos += isGoingUp ? Vector2.up : -Vector2.up;
    }


    public void Update()
    {
        SearchWaitLogic();
    }

    private void SearchWaitLogic()
    {
        if(!isSearching) return;
        timeToWait -= Time.deltaTime;
        
        
        if (!(timeToWait <= 0)) return;
        
        objToSearch.SearchOBJ();
        isSearching = false;
    }
}
