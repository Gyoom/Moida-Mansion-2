using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.Procedural_Generation
{
    public class MansionGenerator
    {
        public int EntranceColumnIndex { get; private set; }
        private readonly List<RoomType> m_alreadyGeneratedRoomType = new List<RoomType>();

        /// <summary>
        /// 0 means false, 1 means two stairs on 1st floor, 2 means two stairs on 3rd floor
        /// </summary>
        private int m_isComplexMansion;

        private const int NumberOfKids = 3;

        public void GenerateMansion(Room[,] mansionMatrix)
        {
            GenerateEntrance(mansionMatrix);
            GenerateOtherRooms(mansionMatrix);
            GenerateStairs(mansionMatrix);
            GenerateDoors(mansionMatrix);

            foreach (var room in mansionMatrix)
            {
                room.Initialize();
            }

            GenerateKids(mansionMatrix);
        }

        private void GenerateEntrance(Room[,] mansionMatrix)
        {
            EntranceColumnIndex = Random.Range(0, 4);
            Room entrance = new GameObject("Entrance").AddComponent<Room>();
            entrance.Type = RoomType.Entrance;
            entrance.ObjInRoom.AddRange(MansionManager.Instance.RoomsData[(int)entrance.Type].PossibleObjInRoom);
            mansionMatrix[EntranceColumnIndex, 1] = entrance;
            m_alreadyGeneratedRoomType.Add(RoomType.Entrance);
            Debug.Log("Entrance generated at " + new Vector2Int(EntranceColumnIndex, 1));
        }

        private void GenerateOtherRooms(Room[,] mansionMatrix)
        {
            for (var x = 0; x < mansionMatrix.GetLength(0); x++)
            for (var y = 0; y < mansionMatrix.GetLength(1); y++)
            {
                int roomTypeIndex = Random.Range(0, 11);
                if (mansionMatrix[x, y] == null)
                {
                    Room room = mansionMatrix[x, y] = new GameObject().AddComponent<Room>();
                    SetRoomType(ref room, roomTypeIndex);
                    room.name = room.Type.ToString();
                    room.ObjInRoom.AddRange(MansionManager.Instance.RoomsData[(int)room.Type].PossibleObjInRoom);
                }
            }
        }

        private void SetRoomType(ref Room room, int index)
        {
            if (room.Type == RoomType.Entrance) return;

            RoomType type = (RoomType)index;

            if (m_alreadyGeneratedRoomType.Contains(type))
            {
                index++;
                if (index > 10)
                {
                    index = m_alreadyGeneratedRoomType.Count == 10
                        ? 0
                        : 1; // if special room has been placed, set default room
                }

                SetRoomType(ref room, index);
                return;
            }

            room.Type = type;
            if (type != RoomType.DefaultRoom)
            {
                m_alreadyGeneratedRoomType.Add(type);
            }
        }

        private void GenerateStairs(Room[,] mansionMatrix)
        {
            GetRoomWithoutStairsInCollum(1, mansionMatrix, out Vector2Int roomPos1).HasStairsUp = true;
            mansionMatrix[roomPos1.x, roomPos1.y + 1].HasStairsDown = true;

            GetRoomWithoutStairsInCollum(1, mansionMatrix, out Vector2Int roomPos2).HasStairsDown = true;
            mansionMatrix[roomPos2.x, roomPos2.y - 1].HasStairsUp = true;

            m_isComplexMansion = Random.Range(0, 3);

            switch (m_isComplexMansion)
            {
                case 1:
                    Room room = GetRoomWithoutStairsInCollum(1, mansionMatrix, out Vector2Int roomPos3);
                    Room roomBelow = mansionMatrix[roomPos3.x, roomPos3.y - 1];

                    room.HasStairsDown = true;
                    roomBelow.HasStairsUp = true;
                    break;
                case 2:
                    Room room4 = GetRoomWithoutStairsInCollum(1, mansionMatrix, out Vector2Int roomPos4);
                    Room roomOver = mansionMatrix[roomPos4.x, roomPos4.y + 1];

                    room4.HasStairsUp = true;
                    roomOver.HasStairsDown = true;
                    break;
            }
        }

        private Room GetRoomWithoutStairsInCollum(int floorIndex, Room[,] mansionMatrix, out Vector2Int roomPos)
        {
            int startValue = Random.Range(0, 4);
            roomPos = new Vector2Int(0, floorIndex);

            for (int i = startValue; i < startValue + 4; i++)
            {
                int index = i;

                if (index > 3) index = i - 4;

                Room room = mansionMatrix[index, floorIndex];
                roomPos = new Vector2Int(i, floorIndex);
                Room roomOver;
                Room roomBelow;

                if (room.Type != RoomType.Entrance && !room.HasStairs())
                {
                    if (floorIndex < 2)
                    {
                        roomOver = mansionMatrix[index, floorIndex + 1];

                        if (roomOver.HasStairs()) continue;
                    }

                    if (floorIndex > 0)
                    {
                        roomBelow = mansionMatrix[index, floorIndex - 1];

                        if (roomBelow.HasStairs()) continue;
                    }

                    return room;
                }
            }

            return null;
        }

        private void GenerateDoors(Room[,] mansionMatrix)
        {
            int randomRoomIndex = Random.Range(0, 4);

            for (var x = 0; x < mansionMatrix.GetLength(0); x++)
            for (var y = 0; y < mansionMatrix.GetLength(1); y++)
            {
                var room = mansionMatrix[x, y];

                switch (m_isComplexMansion)
                {
                    case 0:
                        switch (x)
                        {
                            case 0:
                                room.RightDoor = new Door();
                                break;
                            case 3:
                                room.LeftDoor = new Door();
                                break;
                            default:
                                room.AddBothDoors();
                                break;
                        }

                        break;
                    case 1: // 1st floor has two stairs, so we can add a wall without door in a random room
                        if (x == randomRoomIndex && y == 0)
                        {
                            room.AddOneRandomDoor(randomRoomIndex);
                        }
                        else
                        {
                            switch (x)
                            {
                                case 0:
                                    room.RightDoor = new Door();
                                    break;
                                case 3:
                                    room.LeftDoor = new Door();
                                    break;
                                default:
                                    room.AddBothDoors();
                                    break;
                            }
                        }

                        break;
                    case 2: // Same for 3rd floor
                        if (x == randomRoomIndex && y == 2)
                        {
                            room.AddOneRandomDoor(randomRoomIndex);
                        }
                        else
                        {
                            switch (x)
                            {
                                case 0:
                                    room.RightDoor = new Door();
                                    break;
                                case 3:
                                    room.LeftDoor = new Door();
                                    break;
                                default:
                                    room.AddBothDoors();
                                    break;
                            }
                        }

                        break;
                }
            }
        }

        private void GenerateKids(Room[,] mansionMatrix)
        {
            for (int i = 0; i < NumberOfKids; i++)
            {
                Vector2Int kidPos = new Vector2Int(Random.Range(0, 4), Random.Range(0, 3));

                if (!PutKidIntoContainer(mansionMatrix[kidPos.x, kidPos.y])) i--;
            }
        }

        private bool PutKidIntoContainer(Room room)
        {
            foreach (var roomObj in room.ObjInRoom)
            {
                if (roomObj.CanContainKid && !roomObj.DoContain)
                {
                    roomObj.SetWhatObjContain("Enfant récupéré", new InteractiveObj { IsKid = true });
                    return true;
                }
            }

            return false; // Did not succeed (no valid container in that room)
        }
    }

    public enum RoomType
    {
        Bedroom = 0,
        DefaultRoom = 1, // Room without name
        Dressing = 2,
        Entrance = 3,
        Kitchen = 4,
        Laboratory = 5,
        Library = 6,
        Storage = 7,
        Study = 8,
        Toilet = 9,
        Void = 10,
    }
}