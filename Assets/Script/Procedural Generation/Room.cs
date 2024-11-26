using System.Collections.Generic;
using UnityEngine;

namespace Script.Procedural_Generation
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private List<RoomObj> m_possibleObjInRoom = new List<RoomObj>();
        
        [HideInInspector] public readonly List<RoomObj> ObjInRoom = new List<RoomObj>();

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
    }
}