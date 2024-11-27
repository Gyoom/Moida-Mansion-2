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

            GenerateKids();
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
                int roomTypeIndex = Random.Range(0, 10);
                Room room = mansionMatrix[x, y] = new GameObject().AddComponent<Room>();
                room.name = room.Type.ToString();
                SetRoomType(ref room, roomTypeIndex);
                room.ObjInRoom.AddRange(MansionManager.Instance.RoomsData[(int)room.Type].PossibleObjInRoom);
            }
        }

        private void SetRoomType(ref Room room, int index)
        {
            if (room?.Type == RoomType.Entrance) return;
            
            RoomType type = (RoomType)index;

            if (m_alreadyGeneratedRoomType.Contains(type))
            {
                index++;
                if (index > 10)
                {
                    index = m_alreadyGeneratedRoomType.Count == 10
                        ? 0
                        : 2; // if special room has been placed, set default room
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
            GetRoomWithoutStairsOnFloor(1, mansionMatrix, out Vector2Int roomPos1).HasStairsUp = true;
            mansionMatrix[roomPos1.x, roomPos1.y + 1].HasStairsDown = true;

            GetRoomWithoutStairsOnFloor(1, mansionMatrix, out Vector2Int roomPos2).HasStairsDown = true;
            mansionMatrix[roomPos2.x, roomPos2.y - 1].HasStairsUp = true;

            m_isComplexMansion = Random.Range(0, 3);

            switch (m_isComplexMansion)
            {
                case 1:
                    GetRoomWithoutStairsOnFloor(1, mansionMatrix, out Vector2Int roomPos3).HasStairsDown = true;
                    mansionMatrix[roomPos3.x, roomPos3.y - 1].HasStairsUp = true;
                    break;
                case 2:
                    GetRoomWithoutStairsOnFloor(1, mansionMatrix, out Vector2Int roomPos4).HasStairsUp = true;
                    mansionMatrix[roomPos4.x, roomPos4.y + 1].HasStairsDown = true;
                    break;
            }
        }

        private Room GetRoomWithoutStairsOnFloor(int floorIndex, Room[,] mansionMatrix, out Vector2Int roomPos)
        {
            int startValue = Random.Range(0, 4);

            for (int i = startValue; i < startValue + 4; i++)
            {
                int index = i;

                if (index > 3) index = i - 4;

                Room room = mansionMatrix[index, floorIndex];

                if (!room.HasStairs() && room.Type != RoomType.Entrance)
                {
                    roomPos = new Vector2Int(index, floorIndex);
                    return room;
                }
            }

            roomPos = new Vector2Int(0, floorIndex);
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
                                room.HasRightDoor = true;
                                break;
                            case 3:
                                room.HasLeftDoor = true;
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
                                    room.HasRightDoor = true;
                                    break;
                                case 3:
                                    room.HasLeftDoor = true;
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
                                    room.HasRightDoor = true;
                                    break;
                                case 3:
                                    room.HasLeftDoor = true;
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

        private void GenerateKids()
        {
            for (int i = 0; i < NumberOfKids; i++)
            {
                Vector2Int kidPos = new Vector2Int(Random.Range(0, 4), Random.Range(0, 3));
                
                
            }
        }
    }

    public enum RoomType
    {
        Bedroom,
        DefaultRoom, // Room without name
        Dressing,
        Entrance,
        Kitchen,
        Laboratory,
        Library,
        Storage,
        Study,
        Toilet,
        VoidRoom,
    }
}