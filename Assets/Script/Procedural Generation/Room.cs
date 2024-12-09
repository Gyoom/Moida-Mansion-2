using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Procedural_Generation
{
    public class Room : MonoBehaviour
    {
        public RoomType Type;
        [HideInInspector] public readonly List<RoomObj> ObjInRoom = new List<RoomObj>();
        [HideInInspector] public bool HasStairsUp;
        [HideInInspector] public bool HasStairsDown;
        [HideInInspector] public Door RightDoor;
        [HideInInspector] public Door LeftDoor;

        public void DisplayRoom()
        {
            if (Type != RoomType.DefaultRoom)
            {
                HUDManager.Instance.DisplayStaticText(Type.ToString(), -1, childs.none);
            }
            
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
            LeftDoor = new Door();
            RightDoor = new Door();
        }

        public void AddOneRandomDoor(int roomIndexInFloor)
        {
            switch (roomIndexInFloor)
            {
                case 0:
                    RightDoor = new Door();
                    break;
                case 3:
                    LeftDoor = new Door();
                    break;
                default:
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        LeftDoor = new Door();
                    }
                    else
                    {
                        RightDoor = new Door();
                    }
                    break;
                }
            }
        }

        public void Initialize()
        {
            if (LeftDoor != null)
            {
                ObjInRoom.Add(MansionManager.Instance.CommonRoomObj[0]);
            }

            if (RightDoor != null)
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