using System;
using System.Collections.Generic;
using Script;
using Script.Procedural_Generation;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Monster : MonoBehaviour
{
    public static Monster Instance;
    
    // Appear
    [SerializeField]private bool monsterAppear;
    private float timeToWaitMonster;
    [SerializeField]private int maxTimeToWaitMonster = 2;
    
    [Space]
    // Follow
    [SerializeField]private bool monsterFollow;
    [SerializeField]private int amountRoomFollow = 3;
    [SerializeField]private int amountLeftRoomFollow = 0;
    private bool isfleeing;

    [Space]
    // Monster State
     public Action OnMonsterChazing;
    [SerializeField] private bool isChazing;
    [SerializeField] private float maxTimeBeforeKilling = 2;
    private float timeBeforeKilling = 2;
    public Action OnMonsterKilling;

    [Space]
    // Monster position 
    [SerializeField] private bool isStillInSameRoom;
    [field: SerializeField] public MonsterPosition state { get; private set; }
    [SerializeField] private List<RoomObj> doorStair = new List<RoomObj>();
    [SerializeField] private List<RoomObj> other = new List<RoomObj>();
    private List<RoomObj> monsterPos = new List<RoomObj>();
    private int indexPossibleOBJ;

    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }
    
    private void Start()
    {
        PlayerController.instance.OnSearchBegin += () => timeToWaitMonster = maxTimeToWaitMonster;
        PlayerController.instance.OnSearchEnded += () => monsterAppear = true;
        PlayerController.instance.OnPlayerMove += MonsterFollowPlayer;
        
        ChangeState("Monster_1");
    }
    
    private void MonsterAppear()
    {
        if(MansionManager.Instance.CurrentPlayerRoom().Type == RoomType.Void) return;
        if(isStillInSameRoom) Chazing();
        else
        {
            Debug.Log($"Monster just appear in room {MansionManager.Instance.CurrentPlayerRoom()}");
            DisplayMonster();
        }
    }

    private void MonsterFollowPlayer()
    {
        if(!monsterFollow) return;
        if(MansionManager.Instance.CurrentPlayerRoom().Type == RoomType.Void) return;
        
        isStillInSameRoom = false;
        amountLeftRoomFollow--;
        
        foreach (var obj in monsterPos)
        {
            obj.SetGameObjectActive(false);
        }
        
        if (amountLeftRoomFollow >= 0)
        {
            Debug.Log($"Monster Follow you in room {MansionManager.Instance.CurrentPlayerRoom()}");
            DisplayMonster();
        }
        else
        {
            monsterFollow = false;
            monsterAppear = false;
            state = MonsterPosition.Other;
            isfleeing = false;
        }
    }

    private void DisplayMonster()
    {
        if(monsterPos.Count != 0)
            monsterPos[indexPossibleOBJ].SetGameObjectActive(false);
        monsterPos.Clear();

        var rightDoor = MansionManager.Instance.CurrentPlayerRoom().RightDoor != null;
        var leftDoor = MansionManager.Instance.CurrentPlayerRoom().LeftDoor != null;
        var stairs = MansionManager.Instance.CurrentPlayerRoom().HasStairsDown ||
                     MansionManager.Instance.CurrentPlayerRoom().HasStairsUp;
        
        if (rightDoor && leftDoor)
        {
            monsterPos.Add(doorStair[0]);
            monsterPos.Add(doorStair[1]);
        }
        if (stairs && rightDoor || stairs && leftDoor)
        {
            monsterPos.Add(doorStair[2]);
        }
        
        // add other position
        monsterPos.AddRange(other);
        
        indexPossibleOBJ = Random.Range(0, monsterPos.Count);
        monsterPos[indexPossibleOBJ].SetGameObjectActive(true);
        
        ChangeState(monsterPos[indexPossibleOBJ].name);
        isStillInSameRoom = true;
    }

    public void Chazing()
    {
        if(MansionManager.Instance.CurrentPlayerRoom().Type == RoomType.Void) return;
        isChazing = true;
        timeBeforeKilling = maxTimeBeforeKilling;
        Debug.Log($"The Monsta");
        OnMonsterChazing?.Invoke();
    }

    private void ChangeState(string monsterPosition)
    {
        switch (monsterPosition)
        {
            case "Monster_1":
                state = MonsterPosition.Other;
                break;
            
            case "Monster_2":
                state = MonsterPosition.RightDoor;
                break;
            
            case "Monster_3":
                state = MonsterPosition.LeftDoor;
                break;
            
            case "Monster_4":
                state = MonsterPosition.Other;
                break;
            
            case "Monster_5":
                state = MonsterPosition.Stair;
                break;
            
            case "Monster_6":
                state = MonsterPosition.Other;
                break;
        }
    }
    
    private void MonsterAppearLogic()
    {
        if(!monsterAppear) return;
        timeToWaitMonster -= Time.deltaTime;
        
        if (!(timeToWaitMonster <= 0)) return;
        monsterAppear = false;
        
        Monster.Instance.MonsterAppear();

        monsterFollow = true;
        amountLeftRoomFollow = amountRoomFollow;
    }

    public void PlayerIsFleeing()
    {
        isChazing = false;
        isfleeing = true;
    }

    private void MonsterChazingTimer()
    {
        if(!isChazing) return;
        if(isfleeing)
        {
            isfleeing = false;
            return;
        }
        
        timeBeforeKilling -= Time.deltaTime;
        HUDManager.Instance.DisplayStaticText($"The Monsta", 5, Childs.none);

        if (timeBeforeKilling >= 0) return;
        isChazing = false;
            
        OnMonsterKilling?.Invoke();
        Debug.Log("Death");
    }

    private void Update()
    {
        MonsterAppearLogic();
        MonsterChazingTimer();
    }
}

public enum MonsterPosition
{
    LeftDoor,
    RightDoor,
    Stair,
    Other
}
