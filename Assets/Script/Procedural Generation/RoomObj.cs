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

        public bool GetCanBeSearch()
        {
            return m_canBeSearch;
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
            gameObject.SetActive(true);
            
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
    }
}
