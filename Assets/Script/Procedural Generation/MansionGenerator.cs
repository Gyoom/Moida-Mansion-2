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

        public void GenerateMansion(Room[,] mansionMatrix)
        {
            GenerateEntrance(mansionMatrix);
            GenerateOtherRooms(mansionMatrix);
            GenerateStairs(mansionMatrix);
            GenerateDoors(mansionMatrix);

            foreach (var room in mansionMatrix)
            {
                room.Generate();
            }
        }

        private void GenerateEntrance(Room[,] mansionMatrix)
        {
            EntranceColumnIndex = Random.Range(0, 4);
            mansionMatrix[EntranceColumnIndex, 1] = new GameObject("Entrance").AddComponent<Room>();
            mansionMatrix[EntranceColumnIndex, 1].Type = RoomType.Entrance;
            m_alreadyGeneratedRoomType.Add(RoomType.Entrance);
        }

        private void GenerateOtherRooms(Room[,] mansionMatrix)
        {
            for (var x = 0; x < mansionMatrix.GetLength(0); x++)
            for (var y = 0; y < mansionMatrix.GetLength(1); y++)
            {
                int roomTypeIndex = Random.Range(0, 10);
                mansionMatrix[x, y] = new GameObject().AddComponent<Room>();
                SetRoomType(ref mansionMatrix[x, y].Type, roomTypeIndex);
                mansionMatrix[x, y].name = mansionMatrix[x, y].Type.ToString();
            }
        }

        private void SetRoomType(ref RoomType roomType, int index)
        {
            if (roomType != RoomType.Entrance)
            {
                RoomType type = (RoomType)index;

                if (m_alreadyGeneratedRoomType.Contains(type))
                {
                    index++;
                    if (index > 9)
                    {
                        index = m_alreadyGeneratedRoomType.Count == 9
                            ? 0
                            : 2; // if special room has been placed, set default room
                    }

                    SetRoomType(ref roomType, index);
                    return;
                }

                roomType = type;
                if (type != RoomType.DefaultRoom)
                {
                    m_alreadyGeneratedRoomType.Add(type);
                }
            }
        }

        private void GenerateStairs(Room[,] mansionMatrix)
        {
            GetRoomWithoutStairsOnFloor(1, mansionMatrix).HasStairsUp = true;
            GetRoomWithoutStairsOnFloor(1, mansionMatrix).HasStairsDown = true;

            m_isComplexMansion = Random.Range(0, 3);

            switch (m_isComplexMansion)
            {
                case 1:
                    GetRoomWithoutStairsOnFloor(1, mansionMatrix).HasStairsDown = true;
                    break;
                case 2:
                    GetRoomWithoutStairsOnFloor(1, mansionMatrix).HasStairsUp = true;
                    break;
            }

            for (var x = 0; x < 4; x++)
            {
                var room = mansionMatrix[x, 1];

                if (room.HasStairs())
                {
                    if (room.HasStairsDown) mansionMatrix[x, 0].HasStairsUp = true;
                    else mansionMatrix[x, 2].HasStairsDown = true;
                }
            }
        }

        private Room GetRoomWithoutStairsOnFloor(int floorIndex, Room[,] mansionMatrix)
        {
            int startValue = Random.Range(0, 4);

            for (int i = startValue; i < startValue + 4; i++)
            {
                int index = i;

                if (index > 3) index = i - 4;

                var room = mansionMatrix[index, floorIndex];

                if (!room.HasStairs() && room.Type != RoomType.Entrance)
                {
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
                        room.AddBothDoors();
                        break;
                    case 1: // 1st floor has two stairs, so we can add a wall without door in a random room
                        if (x == randomRoomIndex && y == 0)
                        {
                            room.AddOneRandomDoor(randomRoomIndex);
                        }
                        else
                        {
                            room.AddBothDoors();
                        }
                        break;
                    case 2: // Same for 3rd floor
                        if (x == randomRoomIndex && y == 2) 
                        {
                            room.AddOneRandomDoor(randomRoomIndex);
                        }
                        else
                        {
                            room.AddBothDoors();
                        }
                        break;
                }
            }
        }
    }

    public enum RoomType
    {
        DefaultRoom = 0, // Room without name
        Entrance = 1,
        Storage = 2,
        Librairy = 3,
        Bedroom = 4,
        Study = 5,
        Toilet = 6,
        Dressing = 7,
        Laboratory = 8,
        VoidRoom = 9,
    }
}