using System;
using System.Collections.Generic;
using Script;
using Script.Procedural_Generation;
using UnityEngine;
using Random = UnityEngine.Random;

public class Monster : MonoBehaviour
{
    public static Monster Instance;
    
    // Appear
    private bool monsterAppear;
    private float timeToWaitMonster;
    private int maxTimeToWaitMonster = 2;
    
    // Follow
    private bool monsterFollow;
    private int amountRoomFollow = 3;
    private int amountLeftRoomFollow = 0;
    
    // Monster Position 
    private List<RoomObj> possibleOBJ = new List<RoomObj>();

    private void Start()
    {
        PlayerController.instance.OnSearchBegin += () => timeToWaitMonster = maxTimeToWaitMonster;
        PlayerController.instance.OnSearchEnded += () => monsterAppear = true;
        PlayerController.instance.OnPlayerMove += MonsterFollowPlayer;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }
    
    private void MonsterAppear()
    {
        // TODO : Get the player room location 
        // TODO : Spawn in a random possible monster location 
        
        Debug.Log($"Monster just appear in room {MansionManager.Instance.CurrentPlayerRoom()}");
        DisplayMonster();
    }

    private void MonsterFollowPlayer()
    {
        if(!monsterFollow) return;
        amountLeftRoomFollow--;
        foreach (var obj in possibleOBJ)
        {
            obj.SetGameObjectActive(false);
        }
        
        if (amountLeftRoomFollow >= 0)
        {
            Debug.Log($"Monster Follow you in room {MansionManager.Instance.CurrentPlayerRoom()}");
            DisplayMonster();
        }
        else monsterFollow = false;
    }

    private void DisplayMonster()
    {
        if(possibleOBJ.Count != 0) possibleOBJ.Clear();
        if(MansionManager.Instance.CurrentPlayerRoom().Type == RoomType.Void) return;
        
        possibleOBJ.AddRange(MansionManager.Instance.RoomsData[(int)MansionManager.Instance.CurrentPlayerRoom().Type].PossibleMonsterInRoom);
        
        int randomPosition = Random.Range(0, possibleOBJ.Count);
        possibleOBJ[randomPosition].SetGameObjectActive(true);
    }
    
    private void MonsterLogic()
    {
        if(!monsterAppear) return;
        timeToWaitMonster -= Time.deltaTime;
        
        if (!(timeToWaitMonster <= 0)) return;

        monsterAppear = false;
        Monster.Instance.MonsterAppear();

        monsterFollow = true;
        amountLeftRoomFollow = amountRoomFollow;
    }

    private void Update()
    {
        MonsterLogic();
    }
}
