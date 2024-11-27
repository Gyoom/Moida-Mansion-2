using System.Collections.Generic;
using Script.Procedural_Generation;
using UnityEngine;

namespace Script
{
    public class MansionManager : MonoBehaviour
    {
        public static MansionManager Instance;

        private Room[,] m_mansionMatrix = new Room[4, 3];

        // Serialized for debug
        [SerializeField] private Vector2Int m_playerPosInMansion;
        
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
            m_playerPosInMansion = new Vector2Int(generator.EntranceColumnIndex, 1);
            Debug.Log("Currently in: " + CurrentPlayerRoom());
        }

        public void MovePlayerInMansion(PlayerMove move)
        {
            CurrentPlayerRoom().HideRoom();

            switch (move)
            {
                case PlayerMove.ToLeft:
                    MovePlayerToTheLeft();
                    break;
                case PlayerMove.ToRight:
                    MovePlayerToTheRight();
                    break;
                case PlayerMove.TakeStairs:
                    TakeStairs();
                    break;
            }
            
            CurrentPlayerRoom().DisplayRoom();
        }

        public Room CurrentPlayerRoom()
        {
            return m_mansionMatrix[m_playerPosInMansion.x, m_playerPosInMansion.y];
        }

        private void MovePlayerToTheLeft()
        {
            if (CurrentPlayerRoom().HasLeftDoor)
            {
                m_playerPosInMansion = new Vector2Int(m_playerPosInMansion.x - 1, m_playerPosInMansion.y);
                Debug.Log("Moved to " + CurrentPlayerRoom());
            }
        }

        private void MovePlayerToTheRight()
        {
            if (CurrentPlayerRoom().HasRightDoor)
            {
                m_playerPosInMansion = new Vector2Int(m_playerPosInMansion.x + 1, m_playerPosInMansion.y);
                Debug.Log("Moved to " + CurrentPlayerRoom());
            }
        }

        private void TakeStairs()
        {
            if (CurrentPlayerRoom().HasStairsDown)
            {
                m_playerPosInMansion = new Vector2Int(m_playerPosInMansion.x, m_playerPosInMansion.y -1);
                Debug.Log("Moved to " + CurrentPlayerRoom());
            }
            else if (CurrentPlayerRoom().HasStairsUp)
            {
                m_playerPosInMansion = new Vector2Int(m_playerPosInMansion.x, m_playerPosInMansion.y +1);
                Debug.Log("Moved to " + CurrentPlayerRoom());
            }
        }
        
        public enum PlayerMove
        {
            ToLeft,
            ToRight,
            TakeStairs
        }
    }
}