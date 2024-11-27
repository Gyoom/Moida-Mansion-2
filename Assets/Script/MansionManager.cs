using System.Collections.Generic;
using Script.Procedural_Generation;
using UnityEngine;

namespace Script
{
    public class MansionManager : MonoBehaviour
    {
        public static MansionManager Instance;

        private Room[,] m_mansionMatrix = new Room[4, 3];
        
        public List<RoomObj> CommonRoomObj = new List<RoomObj>();

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            MansionGenerator generator = new MansionGenerator();
            generator.GenerateMansion(m_mansionMatrix);
            m_mansionMatrix[generator.EntranceColumnIndex, 1].DisplayRoom(); // Display Entrance
        }
    }
}