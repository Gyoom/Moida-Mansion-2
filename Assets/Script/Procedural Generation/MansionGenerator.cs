using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Script.Procedural_Generation
{
    public class MansionGenerator
    {
        private Room[,] m_mansionMatrix = new Room[4, 3];
        private int m_entranceColumnIndex;
        private List<RoomType> m_alreadyGeneratedRoomType = new List<RoomType>();

        public void GenerateMansion()
        {
            GenerateEntrance();
            GenerateOtherRooms();
            GenerateStairs();
        }

        private void GenerateEntrance()
        {
            m_entranceColumnIndex = Random.Range(0, 4);
            m_mansionMatrix[m_entranceColumnIndex, 1].Type = RoomType.Entrance;
            m_alreadyGeneratedRoomType.Add(RoomType.Entrance);
        }

        private void GenerateOtherRooms()
        {
            for (var x = 0; x < m_mansionMatrix.GetLength(0); x++)
            for (var y = 0; y < m_mansionMatrix.GetLength(1); y++)
            {
                int roomTypeIndex = Random.Range(0, 10);
                SetRoomType(ref m_mansionMatrix[x, y].Type, roomTypeIndex);
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
                        index = m_alreadyGeneratedRoomType.Count == 8
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

        private void GenerateStairs()
        {
            GetRoomWithoutStairsOnFloor(1).HasStairsUp = true;
            GetRoomWithoutStairsOnFloor(1).HasStairsDown = true;
            
            switch (Random.Range(0, 3)) // Randomly choose if there is a 3rd stair or not, if so, it's either up or down
            {
                case 1: GetRoomWithoutStairsOnFloor(1).HasStairsUp = true;
                    break;
                case 2: GetRoomWithoutStairsOnFloor(1).HasStairsDown = true;
                    break;
            }

            for (var x = 0; x < 4; x++)
            {
                var room = m_mansionMatrix[x, 1];

                if (room.HasStairs())
                {
                    if (room.HasStairsDown) m_mansionMatrix[x, 0].HasStairsUp = true;
                    else m_mansionMatrix[x, 2].HasStairsDown = true;
                }
            }
        }

        private Room GetRoomWithoutStairsOnFloor(int floorIndex)
        {
            int startValue = Random.Range(0, 4);

            for (int i = startValue; i < startValue + 4; i++)
            {
                int index = i;

                if (index > 3) index = i - 4;

                var room = m_mansionMatrix[index, floorIndex];

                if (!room.HasStairs() && room.Type != RoomType.Entrance)
                {
                    return room;
                }
            }

            return null;
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