using System;
using UnityEngine;

namespace Script.Procedural_Generation
{
    public class RoomObj : MonoBehaviour
    {
        // Search
        [SerializeField] private bool m_canBeSearch;
        
        // Contain
        public bool DoContain { get; private set; }
        private string m_containDescription;
        public bool CanContainKid;
        private InteractiveObj m_objToGive;


        public SpriteRenderer sprite { get; private set; }
        private void Awake()
        {
            sprite = GetComponent<SpriteRenderer>();
        }

        public bool GetCanBeSearch()
        {
            return m_canBeSearch;
        }

        public void SetSpriteVisible()
        { 
            sprite.enabled = true;
        }

        // Set on room initialisation 
        public void SetWhatObjContain(string description, InteractiveObj obj)
        {
            DoContain = true;
            m_containDescription = description;
            m_objToGive = obj;
        }

        /// <summary>
        /// Call when obj is inspected 
        /// </summary>
        public void SearchOBJ()
        {
            if(!m_canBeSearch) return; // Just security
            sprite.enabled = true;
            
            switch (DoContain)
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
        private float nextBlinkTime = 0f;
        private float blinkInterval = 0.2f;
        private void SearchBlinkingOBJ()
        {
            if(!isBlinking) return;
            if (Time.time >= nextBlinkTime)
            {
                sprite.enabled = !sprite.enabled;
                nextBlinkTime = Time.time + blinkInterval;
            }
        }
    }
}
