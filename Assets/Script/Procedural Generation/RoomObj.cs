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
        public InteractiveObj m_objToGive;
        
        [SerializeField] private bool isBlinking;
        private float nextBlinkTime = 0f;
        private float blinkInterval = 0.2f;

        // Child Blinking
        public RoomObj noise;


        public SpriteRenderer sprite { get; private set; }
        private void Awake()
        {
            sprite = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            if(noise == null) return;
            if(m_objToGive.kid != Childs.none) return;
            noise.SetObjBlinking();
        }

        private void OnDisable()
        {
            if(noise == null) return;
            if(m_objToGive.kid != Childs.none) return;
            noise.SetObjBlinking(false);
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

        public void SetObjBlinking(bool value = true)
        {
            isBlinking = value;
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
                    //Debug.Log($"{gameObject.name} contain {m_containDescription}");
                    
                    if(m_objToGive == null)break;
                    Debug.Log($"You receive {m_objToGive}");
                    HUDManager.Instance.DisplayStaticText($"{m_objToGive}", 5, m_objToGive.kid);
                    PlayerController.instance.OnFoundChild(m_objToGive);
                    
                    if(m_objToGive.kid != Childs.none) break;
                    noise.SetObjBlinking(false);
                    break;
                
                default:
                    Debug.Log($"{gameObject.name} contain Nothing");
                    HUDManager.Instance.DisplayStaticText($"Nothing", 5, m_objToGive.kid);
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
