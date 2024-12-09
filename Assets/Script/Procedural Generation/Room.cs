using System.Collections.Generic;
using UnityEngine;

namespace Script.Procedural_Generation
{
    public class Room : MonoBehaviour
    {
        public RoomType Type;
        [HideInInspector] public readonly List<RoomObj> ObjInRoom = new List<RoomObj>();
        [HideInInspector] public bool HasStairsUp;
        [HideInInspector] public bool HasStairsDown;
        [HideInInspector] public bool HasRightDoor;
        [HideInInspector] public bool HasLeftDoor;

        public void DisplayRoom()
        {
            foreach (var roomObj in ObjInRoom)
            {
                roomObj.gameObject.SetActive(true);
                roomObj.SetSpriteVisible();
            }
        }

        public void HideRoom()
        {
            foreach (var roomObj in ObjInRoom)
            {
                roomObj.gameObject.SetActive(false);
            }
        }

        public void AddBothDoors()
        {
            HasLeftDoor = true;
            HasRightDoor = true;
        }

        public void AddOneRandomDoor(int roomIndexInFloor)
        {
            switch (roomIndexInFloor)
            {
                case 0:
                    HasRightDoor = true;
                    break;
                case 3:
                    HasLeftDoor = true;
                    break;
                default:
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        HasLeftDoor = true;
                    }
                    else
                    {
                        HasRightDoor = true;
                    }
                    break;
                }
            }
        }

        public void Initialize()
        {
            if (HasLeftDoor)
            {
                ObjInRoom.Add(MansionManager.Instance.CommonRoomObj[0]);
            }

            if (HasRightDoor)
            {
                ObjInRoom.Add(MansionManager.Instance.CommonRoomObj[1]);
            }
            
            if (HasStairsDown)
            {
                ObjInRoom.Add(MansionManager.Instance.CommonRoomObj[2]);
                ObjInRoom.Add(MansionManager.Instance.CommonRoomObj[4]);
            }
            else if (HasStairsUp)
            {
                ObjInRoom.Add(MansionManager.Instance.CommonRoomObj[2]);
                ObjInRoom.Add(MansionManager.Instance.CommonRoomObj[3]);
            }
        }

        public bool HasStairs() => HasStairsUp && HasStairsDown;
    }
}