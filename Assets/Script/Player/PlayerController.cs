using System;
using Script;
using Script.Procedural_Generation;
using UnityEngine;

public class PlayerController : Actor
{
    public static PlayerController instance;

    private bool isSearching;
    private float timeToWait;
    private int maxTimeToWait = 5;

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
        
        Debug.Log($"Move Right !");
        
        //TODO : Draw the room you at, update position on array 
        
        MansionManager.Instance.MovePlayerInMansion(MansionManager.PlayerMove.ToRight);
    }

    public override void MoveLeft()
    {
        isSearching = false;
        
        Debug.Log($"MoveLeft !");
        
        MansionManager.Instance.MovePlayerInMansion(MansionManager.PlayerMove.ToLeft);
    }

    public override void Search()
    {
        Debug.Log($"Search !");
        isSearching = true;
        timeToWait = maxTimeToWait;
        
        if(objToSearch != null)
            objToSearch.gameObject.SetActive(true); // security to ensure its enable (blink)
        
        objToSearch = SearchTest.instance.GetAObjToSearch();
        
    }

    public override void TakeStair()
    {
        isSearching = false;
        
        // TODO : get the position of where it lead 

        Debug.Log($"TakeStair !");
        
        MansionManager.Instance.MovePlayerInMansion(MansionManager.PlayerMove.TakeStairs);
    }


    public void Update()
    {
        SearchWaitLogic();
    }

    private void SearchWaitLogic()
    {
        if(!isSearching) return;
        timeToWait -= Time.deltaTime;

        SearchBlinkingOBJ();
        if (!(timeToWait <= 0)) return;
        
        objToSearch.SearchOBJ();
        isSearching = false;
    }

    private float blinkInterval = 0.5f; 
    private float nextBlinkTime = 0f; 
    private void SearchBlinkingOBJ()
    {
        if (Time.time >= nextBlinkTime)
        {
            objToSearch.gameObject.SetActive(!objToSearch.gameObject.activeSelf);
            nextBlinkTime = Time.time + blinkInterval;
        }
    }
}
