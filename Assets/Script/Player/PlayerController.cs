using System;
using System.Collections.Generic;
using Script;
using Script.Procedural_Generation;
using UnityEngine;

public class PlayerController : Actor
{
    public static PlayerController instance;

    // Step
    public int stepAmount { get; private set; }
    
    // Search
    public int searchAmount { get; private set; }
    private bool isSearching;
    private float timeToWait;
    private int maxTimeToWait = 5;
    
    // Monster
    private bool monsterAppear;
    private float timeToWaitMonster;
    private int maxTimeToWaitMonster = 2;

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
        stepAmount++;
        isSearching = false;
        
        Debug.Log($"Try Move Right !");
        
        MansionManager.Instance.MovePlayerInMansion(MansionManager.PlayerMove.ToRight);
//        SoundManager.Instance.SpawnAudio3D(transform.position, 1);
        allObjsToSearch.Clear();
    }

    public override void MoveLeft()
    {
        stepAmount++;
        isSearching = false;
        
        Debug.Log($"Try MoveLeft !");
        
        MansionManager.Instance.MovePlayerInMansion(MansionManager.PlayerMove.ToLeft);
        allObjsToSearch.Clear();
    }

    
    private List<RoomObj> allObjsToSearch = new List<RoomObj>();
    private int index = 0;
    
    public override void Search()
    {
        Debug.Log($"Try Search !");
        isSearching = true;
        searchAmount++;
        timeToWait = maxTimeToWait;
        timeToWaitMonster = maxTimeToWaitMonster;
        
        if(objToSearch != null && allObjsToSearch.Count != 0)
            objToSearch.sprite.enabled = true; // security to ensure blink
        
        
        foreach (var obj in MansionManager.Instance.CurrentPlayerRoom().ObjInRoom)
        {
            if(obj.GetCanBeSearch()) allObjsToSearch.Add(obj);
        }
        
        int maxIndex = allObjsToSearch.Count;
        if (index < maxIndex)
        {
            objToSearch = allObjsToSearch[index];
        }
        else
        {
            index = 0;
            objToSearch = allObjsToSearch[index];
        }
        index++;
    }

    public override void TakeStair()
    {
        isSearching = false;
        stepAmount++;

        Debug.Log($"Try TakeStair !");
        
        MansionManager.Instance.MovePlayerInMansion(MansionManager.PlayerMove.TakeStairs);
        allObjsToSearch.Clear();
    }


    public void Update()
    {
        SearchWaitLogic();
        MonsterLogic();
    }

    private void SearchWaitLogic()
    {
        if(!isSearching) return;
        timeToWait -= Time.deltaTime;

        SearchBlinkingOBJ();
        if (!(timeToWait <= 0)) return;
        
        objToSearch.SearchOBJ();
        isSearching = false;
        monsterAppear = true;
    }

    private float blinkInterval = 0.2f; 
    private float nextBlinkTime = 0f; 
    private void SearchBlinkingOBJ()
    {
        if (Time.time >= nextBlinkTime)
        {
            objToSearch.sprite.enabled = !objToSearch.sprite.enabled;
            nextBlinkTime = Time.time + blinkInterval;
        }
        //else objToSearch.sprite.enabled = true;
    }

    private void MonsterLogic()
    {
        if(!monsterAppear) return;
        timeToWaitMonster -= Time.deltaTime;
        if (!(timeToWaitMonster <= 0)) return;

        monsterAppear = false;
        Monster.Instance.MonsterAppear();
    }
}
