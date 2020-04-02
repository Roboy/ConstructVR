using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Construct.Interaction
{
    /// <summary>
    /// Holds References to all GameObjects of the type Ball, will be transformed into ObjectManager in the future.
    /// </summary>
    public class BallManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private List<GameObject> m_Balls = new List<GameObject>();
        [SerializeField]
        private Dictionary<int, GameObject> m_InteractableObjects = new Dictionary<int, GameObject>();

        // Start is called before the first frame update
        void Start()
        {
            InitializeBalls();
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Adds Interactable GameObjects to a reference list. Assings IDs automatically.
        /// </summary>
        void InitializeBalls()
        {
            int id = 1;
            //Search through all children
            foreach (Transform child in transform)
            {
                //If it is interactable, add it to the list
                if (child.GetComponent<InteractableCustom>())
                {
                    child.GetComponent<InteractableCustom>().SetID(id);
                    m_Balls.Add(child.gameObject);
                    m_InteractableObjects.Add(id, child.gameObject);
                }

                id++;
            }
        }
        /// <summary>
        /// Returns the GameObject by its ID.
        /// </summary>
        /// <param name="ID">ID of the GameObject.</param>
        /// <returns></returns>
        public GameObject LookUpObjectByID(int ID)
        {
            if (m_InteractableObjects.ContainsKey(ID))
            {
                return m_InteractableObjects[ID];
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// Used when GameObjects are removed to ensure consistency in the scenes references.
        /// </summary>
        /// <param name="go">Which GameObject should be removed.</param>
        /// <param name="ID">Whats the ID of the GameObject to be removed.</param>
        public void UpdateReferences(GameObject go, int ID)
        {
            m_Balls.Remove(go);
            m_InteractableObjects.Remove(ID);
        }
    }

}
