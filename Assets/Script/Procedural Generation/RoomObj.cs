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
        
        [SerializeField] private bool isBlinking;
        private float nextBlinkTime = 0f;
        private float blinkInterval = 0.2f;

        // Child Blinking
        public RoomObj child;


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
            if (sprite != null)
            {
                sprite.enabled = true;
            }
        }

        public void SetGameObjectActive(bool value)
        {
            gameObject.SetActive(value);
        }

        public void SetObjBlinking()
        {
            isBlinking = true;
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
                    HUDManager.Instance.DisplayStaticText($"You receive {m_objToGive}", 5, childs.none);
                    break;
                
                default:
                    Debug.Log($"{gameObject.name} contain Nothing");
                    HUDManager.Instance.DisplayStaticText($"{gameObject.name} contain Nothing", 5, childs.none);
                    break;
            }
        }

        private void Update()
        {
            SearchBlinkingOBJ();
        }
        
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
