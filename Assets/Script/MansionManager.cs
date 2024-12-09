using System;
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

        [Space] public List<RoomObj> CommonRoomObj = new List<RoomObj>();

        [Space] public List<RoomData> RoomsData = new List<RoomData>();

        private void Awake()
        {
            Instance = this;
        }

        public void StartGame()
        {
            MansionGenerator generator = new MansionGenerator();
            generator.GenerateMansion(m_mansionMatrix);
            m_mansionMatrix[generator.EntranceColumnIndex, 1].DisplayRoom(); // Display Entrance
            m_playerPosInMansion = new Vector2Int(generator.EntranceColumnIndex, 1);
            Debug.Log("Currently in: " + CurrentPlayerRoom());
            HUDManager.Instance.UpdateMap(true, m_playerPosInMansion);
            HUDManager.Instance.updateInputs();
        }

        public void MovePlayerInMansion(PlayerMove move)
        {
            HUDManager.Instance.DisplayStaticText(string.Empty, -1, childs.none);
            
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

            HUDManager.Instance.UpdateMap(true, m_playerPosInMansion);
            HUDManager.Instance.updateInputs();
        }

        public Room CurrentPlayerRoom()
        {
            return m_mansionMatrix[m_playerPosInMansion.x, m_playerPosInMansion.y];
        }

        private void MovePlayerToTheLeft()
        {
            if (CurrentPlayerRoom().HasLeftDoor)
            {
                CurrentPlayerRoom().HideRoom();
                m_playerPosInMansion = new Vector2Int(m_playerPosInMansion.x - 1, m_playerPosInMansion.y);
                HUDManager.Instance.OnMoveTransition += CurrentPlayerRoom().DisplayRoom;
                LogPlayerPos();
                StartCoroutine(HUDManager.Instance.Transition(Dir.left));
            }
        }

        private void MovePlayerToTheRight()
        {
            if (CurrentPlayerRoom().HasRightDoor)
            {
                CurrentPlayerRoom().HideRoom();
                m_playerPosInMansion = new Vector2Int(m_playerPosInMansion.x + 1, m_playerPosInMansion.y);
                HUDManager.Instance.OnMoveTransition += CurrentPlayerRoom().DisplayRoom;
                LogPlayerPos();
                StartCoroutine(HUDManager.Instance.Transition(Dir.right));
            }
        }

        private void LogPlayerPos()
        {
            Debug.Log($"Moved to {CurrentPlayerRoom()}, new player pos : {m_playerPosInMansion}");
        }
        
        private void TakeStairs()
        {
            if (CurrentPlayerRoom().HasStairsDown)
            {
                CurrentPlayerRoom().HideRoom();
                m_playerPosInMansion = new Vector2Int(m_playerPosInMansion.x, m_playerPosInMansion.y - 1);
                HUDManager.Instance.OnMoveTransition += CurrentPlayerRoom().DisplayRoom;
                LogPlayerPos();
                StartCoroutine(HUDManager.Instance.Transition(Dir.down));
            }
            else if (CurrentPlayerRoom().HasStairsUp)
            {
                CurrentPlayerRoom().HideRoom();
                m_playerPosInMansion = new Vector2Int(m_playerPosInMansion.x, m_playerPosInMansion.y + 1);
                HUDManager.Instance.OnMoveTransition += CurrentPlayerRoom().DisplayRoom;
                LogPlayerPos();
                StartCoroutine(HUDManager.Instance.Transition(Dir.top));
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