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
        public bool isBtn;
        private InteractiveObj m_objToGive;

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
            if (noise == null) return;
            if (m_objToGive == null) return;
            if (m_objToGive.kid != Childs.none && m_objToGive.kidRoom == MansionManager.Instance.CurrentPlayerRoom())
            {
                noise.SetObjBlinking();
            }
        }

        private void OnDisable()
        {
            if (noise == null) return;
            noise.SetObjBlinking(false);
        }

        public bool GetCanBeSearch()
        {
            return m_canBeSearch;
        }

        public void SetSpriteVisible(bool value = true)
        {
            if (sprite != null)
            {
                sprite.enabled = value;
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
            if (!m_canBeSearch) return; // Just security
            sprite.enabled = true;

            if (isBtn)
            {
                PlayerController.instance.OnClickBtn?.Invoke();
                return;
            }

            switch (DoContain)
            {
                case true:
                    //Debug.Log($"{gameObject.name} contain {m_containDescription}");

                    if (m_objToGive == null) break;
                    if (isBtn)
                    {
                        PlayerController.instance.OnClickBtn?.Invoke();
                    }
                    else
                    {
                        if (m_objToGive.kid != Childs.none &&
                            m_objToGive.kidRoom == MansionManager.Instance.CurrentPlayerRoom())
                        {
                            Debug.Log($"You receive {m_objToGive}");
                            //HUDManager.Instance.DisplayStaticText($"{m_objToGive}", 5, m_objToGive.kid);
                            PlayerController.instance.OnFoundChild(m_objToGive);
                            RemoveContainedChild();
                        }
                        else
                        {
                            HUDManager.Instance.DisplayStaticText($"Nothing", 5, Childs.none);
                        }
                    }

                    break;

                default:
                    Debug.Log($"{gameObject.name} contain Nothing");
                    if (m_objToGive != null)
                    {
                        HUDManager.Instance.DisplayStaticText($"Nothing", 5, m_objToGive.kid);
                    }
                    else
                    {
                        HUDManager.Instance.DisplayStaticText($"Nothing", 5, Childs.none);
                    }

                    break;
            }
        }

        private void RemoveContainedChild()
        {
            m_objToGive.kid = Childs.none;
            m_objToGive.kidRoom = null;
            noise.SetObjBlinking(false);
            noise.SetSpriteVisible(false);
        }
        
        private void Update()
        {
            SearchBlinkingOBJ();
        }

        private void SearchBlinkingOBJ()
        {
            if (!isBlinking) return;
            if (Time.time >= nextBlinkTime)
            {
                sprite.enabled = !sprite.enabled;
                nextBlinkTime = Time.time + blinkInterval;
            }
        }
    }
}