using System;
using UnityEngine;

namespace Script.Procedural_Generation
{
    public class RoomObj : MonoBehaviour
    {
        // Search
        [SerializeField] private bool m_canBeSearch;
        
        // Contain
        private bool m_doContain;
        private string m_containDescription;
        public bool CanContainKid;
        private InteractiveObj m_objToGive;

        public bool GetCanBeSearch()
        {
            return m_canBeSearch;
        }

        // Set on room initialisation 
        public void SetWhatObjContain(string description, InteractiveObj obj)
        {
            m_doContain = true;
            m_containDescription = description;
            m_objToGive = obj;
        }

        /// <summary>
        /// Call when obj is inspected 
        /// </summary>
        public void SearchOBJ()
        {
            if(!m_canBeSearch) return; // Just security
            gameObject.SetActive(true);
            
            switch (m_doContain)
            {
                case true:
                    Debug.Log($"{gameObject.name} contain {m_containDescription}");
                    
                    if(m_objToGive == null)break;
                    Debug.Log($"You receive {m_objToGive}");
                    break;
                
                default:
                    Debug.Log($"{gameObject.name} contain Nothing");
                    break;
            }
        }

        private void Update()
        {
            SearchBlinkingOBJ();
        }

        [SerializeField] private bool isBlinking;
        private float blinkInterval = 0.2f; 
        private float nextBlinkTime = 0f; 
        private void SearchBlinkingOBJ()
        {
            if(!isBlinking) return;
            if (Time.time >= nextBlinkTime)
            {
                gameObject.SetActive(!gameObject.activeSelf);
                nextBlinkTime = Time.time + blinkInterval;
            }
        }
    }
}
