using System;
using Script;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public static Monster Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }


    public void MonsterAppear()
    {
        // TODO : Get the player room location 
        // TODO : Spawn in a random possible monster location 
        
        //Debug.Log($"Monster just appear in room {MansionManager.Instance.}");
    }
}
