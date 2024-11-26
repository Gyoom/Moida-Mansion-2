using UnityEngine;

namespace Script.Procedural_Generation
{
    public class RoomObj : MonoBehaviour
    {
        //TODO faire une variable "content" si jamais l'obj contient qqchose
        
        //TODO faire une variable pour savoir si il faut un obj pour intéragir avec cette objet

        // Search
        [SerializeField] private bool canBeSearch;
        
        // Contain
        private bool doContain = false; // is OBJ contain Something
        private string containDescription; // description of what it contains
        private RoomObj objToGive;


        // Value of canBeSearch
        public bool GetCanBeSearch()
        {
            return canBeSearch;
        }

        // Set on room initialisation 
        public void SetWhatObjContain(string description, RoomObj OBJ)
        {
            doContain = true;
            objToGive = OBJ;
        }

        /// <summary>
        /// Call when obj is inspected 
        /// </summary>
        public void SearchOBJ()
        {
            switch (doContain)
            {
                case true:
                    Debug.Log($"it contain {containDescription}");
                    
                    if(objToGive == null)break;
                    Debug.Log($"You receive {objToGive}");
                    break;
                
                default:
                    Debug.Log("Nothing");
                    break;
            }
        }
    }
}
