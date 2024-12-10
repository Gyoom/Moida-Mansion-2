using System.Collections.Generic;
using Script.Procedural_Generation;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script
{
    public class MansionManager : MonoBehaviour
    {
        public static MansionManager Instance;

        public Room[,] MansionMatrix = new Room[4, 3];

        // Serialized for debug
        [SerializeField] private Vector2Int m_playerPosInMansion;

        [Space] public List<RoomObj> CommonRoomObj = new List<RoomObj>();

        [Space] public List<RoomData> RoomsData = new List<RoomData>();
         
        public int ActivatedButtons;

        private void Awake()
        {
            Instance = this;
        }

        public void StartGame()
        {
            MansionGenerator generator = new MansionGenerator();
            generator.GenerateMansion(MansionMatrix);
            MansionMatrix[generator.EntranceColumnIndex, 1].DisplayRoom(); // Display Entrance
            m_playerPosInMansion = new Vector2Int(generator.EntranceColumnIndex, 1);
            Debug.Log("Currently in: " + CurrentPlayerRoom());
            HUDManager.Instance.UpdateMap(true, m_playerPosInMansion);
            HUDManager.Instance.UpdateInputs(CurrentPlayerRoom());
        }

        public void MovePlayerInMansion(PlayerMove move)
        {
            HUDManager.Instance.DisplayStaticText(string.Empty, -1, Childs.none);

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
            HUDManager.Instance.UpdateInputs(CurrentPlayerRoom());
        }

        public Room CurrentPlayerRoom()
        {
            return MansionMatrix[m_playerPosInMansion.x, m_playerPosInMansion.y];
        }

        private void MovePlayerToTheLeft()
        {
            if (CurrentPlayerRoom().LeftDoor != null)
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
            if (CurrentPlayerRoom().RightDoor != null)
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