using System.Collections.Generic;
using Script.Procedural_Generation;
using UnityEngine;

namespace Script
{
    public class MansionManager : MonoBehaviour
    {
        public static MansionManager Instance;

        private Room[,] m_mansionMatrix = new Room[4, 3];
        
        [Space]
        public List<RoomObj> CommonRoomObj = new List<RoomObj>();

        [Space]
        public List<RoomData> RoomsData = new List<RoomData>();

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

        public void MovePlayerInMansion(PlayerMove move)
        {
            
        }

        public enum PlayerMove
        {
            ToLeft,
            ToRight,
            TakeStairs
        }
    }
}