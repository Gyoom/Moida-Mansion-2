using System;
using System.Collections.Generic;

namespace Script.Procedural_Generation
{
    [Serializable]
    public class RoomData
    {
        public string RoomName;
        public List<RoomObj> PossibleObjInRoom = new List<RoomObj>();
        public List<RoomObj> PossibleMonsterInRoom = new List<RoomObj>();
    }
}