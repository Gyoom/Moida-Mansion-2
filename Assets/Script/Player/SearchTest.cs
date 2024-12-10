using System;
using System.Collections.Generic;
using Script.Procedural_Generation;
using UnityEngine;

public class SearchTest : MonoBehaviour
{

    public List<RoomObj> objToSearch;
    private int index = 0;

    public static SearchTest instance;

    private void Awake()
    {
        instance = this;
    }
    
}
