using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Procedural_Generation
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private List<RoomObj> m_possibleObjInRoom = new List<RoomObj>();
        public RoomType Type;
        [HideInInspector] public readonly List<RoomObj> ObjInRoom = new List<RoomObj>();
        [HideInInspector] public bool HasStairsUp;
        [HideInInspector] public bool HasStairsDown;
        
        public void DisplayRoom()
        {
            foreach (var roomObj in ObjInRoom)
            {
                roomObj.gameObject.SetActive(true);
            }
        }

        public void HideRoom()
        {
            foreach (var roomObj in ObjInRoom)
            {
                roomObj.gameObject.SetActive(false);
            }
        }

        public void GenerateRoom()
        {
            
        }

        public bool HasStairs() => HasStairsUp && HasStairsDown;
    }
}