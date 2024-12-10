using System.Collections.Generic;
using System.Numerics;

namespace Script.Procedural_Generation
{
    public class InteractiveObj
    {
        public Childs kid = Childs.none;
        public Room kidRoom; 
        public List<string> dialogue = new List<string>();
    }
}