using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Script.Procedural_Generation
{
    public class MansionGenerator
    {
        private RoomType[,] m_mansionMatrix = new RoomType[4, 3];
        private List<RoomType> m_alreadyGeneratedRoomType = new List<RoomType>();

        public void GenerateMansion()
        {
            GenerateEntrance();
            GenerateOtherRooms();
        }

        private void GenerateEntrance()
        {
           int entranceColumnIndex = Random.Range(0, 4);
           m_mansionMatrix[entranceColumnIndex, 1] = RoomType.Entrance;
           m_alreadyGeneratedRoomType.Add(RoomType.Entrance);
        }

        private void GenerateOtherRooms()
        {
            for (var x = 0; x < m_mansionMatrix.GetLength(0); x++)
            for (var y = 0; y < m_mansionMatrix.GetLength(1); y++)
            {
                int roomTypeIndex = Random.Range(0, 10);
                SetRoomType(ref m_mansionMatrix[x, y], roomTypeIndex);
            }
        }

        private void SetRoomType(ref RoomType roomType, int index)
        {
            if (roomType != RoomType.Entrance)
            {
                RoomType type = (RoomType) index;
                
                if (m_alreadyGeneratedRoomType.Contains(type))
                {
                    index++;
                    if (index > 9)
                    {
                        index = m_alreadyGeneratedRoomType.Count == 8 ? 0 : 2; // if special room has been placed, set default room
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