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


    public RoomObj GetAObjToSearch()
    {
        RoomObj objToReturn;
        int maxIndex = objToSearch.Count;

        if (index < maxIndex)
        {
            objToReturn = objToSearch[index];
        }
        else
        {
            index = 0;
            objToReturn = objToSearch[index];
        }
        
        if(index == 1) objToSearch[index].SetWhatObjContain("Un super cadeau", null);

        index++;
        return objToReturn;

    }
    
}
