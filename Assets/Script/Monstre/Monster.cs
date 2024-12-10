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
    [SerializeField]private bool monsterAppear;
    private float timeToWaitMonster;
    [SerializeField]private int maxTimeToWaitMonster = 2;
    
    [Space]
    // Follow
    [SerializeField]private bool monsterFollow;
    [SerializeField]private int amountRoomFollow = 3;
    [SerializeField]private int amountLeftRoomFollow = 0;

    [Space]
    // Monster State
    public Action OnMonsterChazing;
    [SerializeField] private bool isChazing;
    [SerializeField] private float maxTimeBeforeKilling = 2;
    private float timeBeforeKilling = 2;
    public Action OnMonsterKilling;
    
    [Space]
    // Monster position 
    [SerializeField]private MonsterPosition state;
    private List<RoomObj> possibleOBJ = new List<RoomObj>();
    private int indexPossibleOBJ;

    private void Start()
    {
        PlayerController.instance.OnSearchBegin += () => timeToWaitMonster = maxTimeToWaitMonster;
        PlayerController.instance.OnSearchEnded += () => monsterAppear = true;
        PlayerController.instance.OnPlayerMove += MonsterFollowPlayer;
        
        ChangeState("Monster_1");
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
        else
        {
            monsterFollow = false;
            monsterAppear = false;
        }
    }

    private void DisplayMonster()
    {
        if(possibleOBJ.Count != 0) possibleOBJ.Clear();
        if(MansionManager.Instance.CurrentPlayerRoom().Type == RoomType.Void) return;

        if (monsterFollow)
        {
            Chazing();
        }
        else
        {
            possibleOBJ.AddRange(MansionManager.Instance.RoomsData[(int)MansionManager.Instance.CurrentPlayerRoom().Type].PossibleMonsterInRoom);
            possibleOBJ[indexPossibleOBJ].SetGameObjectActive(false);
        
            indexPossibleOBJ = Random.Range(0, possibleOBJ.Count);
            possibleOBJ[indexPossibleOBJ].SetGameObjectActive(true);
        
            ChangeState(possibleOBJ[indexPossibleOBJ].name);
        }
    }

    private void Chazing()
    {
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

    private void MonsterChazingTimer()
    {
        if(!isChazing) return;
        timeBeforeKilling -= Time.deltaTime;

        if (!(timeBeforeKilling <= 0)) return;
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
    RightDoor,
    LeftDoor,
    Stair,
    Other
}
