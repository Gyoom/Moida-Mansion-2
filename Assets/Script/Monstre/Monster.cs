using System;
using Script;
using UnityEngine;

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
    }

    private void MonsterFollowPlayer()
    {
        if(!monsterFollow) return;
        amountLeftRoomFollow--;
        if (amountLeftRoomFollow >= 0)
        {
            Debug.Log($"Monster Follow you in room {MansionManager.Instance.CurrentPlayerRoom()}");
        }
        else monsterFollow = false;
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
