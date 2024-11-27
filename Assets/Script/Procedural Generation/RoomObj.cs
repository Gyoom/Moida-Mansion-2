using System;
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
        private bool doContain = false;
        private string containDescription; 
        private RoomObj objToGive;

        
        public bool GetCanBeSearch()
        {
            return canBeSearch;
        }
        
        public void SetWhatObjContain(string description, RoomObj OBJ)
        {
            doContain = true;
            containDescription = description;
            objToGive = OBJ;
        }
        
        public void SearchOBJ()
        {
            if(!canBeSearch) return; // Just security
            gameObject.SetActive(true);
            
            switch (doContain)
            {
                case true:
                    Debug.Log($"{gameObject.name} contain {containDescription}");
                    
                    if(objToGive == null)break;
                    Debug.Log($"You receive {objToGive}");
                    break;
                
                default:
                    Debug.Log($"{gameObject.name} contain Nothing");
                    break;
            }
        }

        private void Update()
        {
            
        }
        
        private float blinkInterval = 0.5f; 
        private float nextBlinkTime = 0f; 
        private void SearchBlinkingOBJ()
        {
            if (Time.time >= nextBlinkTime)
            {
                gameObject.SetActive(!gameObject.activeSelf);
                nextBlinkTime = Time.time + blinkInterval;
            }
        }
    }
}
