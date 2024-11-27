using System.Collections.Generic;
using UnityEngine;

namespace Script.Procedural_Generation
{
    [CreateAssetMenu(fileName = "RoomData", menuName = "ScriptableObjects/RoomData", order = 1)] 
    public class RoomData : ScriptableObject
    {
        [SerializeField] private List<RoomObj> m_possibleObjInRoom = new List<RoomObj>();
    }
}