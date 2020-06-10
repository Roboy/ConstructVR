using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Construct.Interaction
{
    /// <summary>
    /// EyeTracker checks for GameObjects that the user looks at. If it is of the type "Interactable", it will be highlighted by an outline.
    /// This selected target can then be removed by blinking, triggering a physics effect.
    /// </summary>
    public class EyeTracker : MonoBehaviour
    {
        #region VAR

        [Header("Attributes")]
        [Range(0.0f, 1.0f)]
        public float OutlineThickness;
        [SerializeField]
        private Vector2 m_MousePosition;

        [Header("Explosion")]
        [Range(50.0f, 850.0f)]
        public float ExplosionForce = 200.0f;
        [Range(0.5f, 5.0f)]
        public float ExplosionRadius = 4.0f;

        [Header("References")]
        public BallManager BM;
        [SerializeField]
        private Camera m_MainCamera;
        [SerializeField]
        private GameObject m_LockedTarget;
        private bool m_LookingForTarget = false;

        #endregion


        void Start()
        {
            if( GameObject.FindGameObjectWithTag("MainCamera") == null) 
            {
                m_MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            }
            
        }


        void Update()
        {
            //Workaround for now, eye tracking position will replace this
            m_MousePosition = Input.mousePosition;

            if (m_LookingForTarget == false)
            {
                m_LookingForTarget = true;
                StartCoroutine(CheckForTarget());
            }


            //Blinking will replace the button press in the future
            //This action removes the locked target gameObject and leaves a force effect in the area
            if (Input.GetKeyDown(KeyCode.X))
            {
                RemoveTarget();
            }

        }


        /// <summary>
        /// Identifies the object the user is looking at. Weights a number of raycasts in a period of time and identifies the most frequent object.
        /// </summary>
        /// <returns></returns>
        IEnumerator CheckForTarget()
        {
            //Variables
            float Timer = 0.4f;
            float timeStep = 0.1f;
            float currentTime = 0.0f;

            RaycastHit hit;
            Ray ray;
            List<int> hitIDs = new List<int>();

            #region Raycasting
            //Checking at what objects the user looks in a period of time
            while (currentTime < Timer)
            {
                currentTime += timeStep;
                ray = m_MainCamera.ScreenPointToRay(m_MousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    Transform objectHit = hit.transform;

                    if (hit.transform.GetComponent<InteractableCustom>() != null)
                    {
                        int id = hit.transform.GetComponent<InteractableCustom>().GetID();
                        hitIDs.Add(id);
                    }

                }

                yield return new WaitForSeconds(timeStep);
            }

            #endregion

            #region ResultEval
            GameObject ResultObject = CheckForMostFrequent(hitIDs);
            //In case the result is negative, remove outline from former locked target
            if (ResultObject == null)
            {
                if (m_LockedTarget)
                {
                    m_LockedTarget.GetComponent<Renderer>().material.SetFloat("_FirstOutlineWidth", 0.0f);
                    m_LockedTarget = null;
                }
            }
            else
            {
                //Current locked target is different from new one
                if (m_LockedTarget != ResultObject)
                {
                    //Only change the outline if not null
                    if (m_LockedTarget)
                    {
                        m_LockedTarget.GetComponent<Renderer>().material.SetFloat("_FirstOutlineWidth", 0.0f);
                        m_LockedTarget = ResultObject;
                        m_LockedTarget.GetComponent<Renderer>().material.SetFloat("_FirstOutlineWidth", OutlineThickness);
                    }
                }
                //Before the current check there was no locked target
                if (m_LockedTarget == null)
                {
                    m_LockedTarget = ResultObject;
                    m_LockedTarget.GetComponent<Renderer>().material.SetFloat("_FirstOutlineWidth", OutlineThickness);
                }

            }
            #endregion

            //Exit the search
            m_LookingForTarget = false;
            yield return null;
        }

        /// <summary>
        /// Samples through hit objects in time period and calculates the most frequent one
        /// </summary>
        private GameObject CheckForMostFrequent(List<int> hitIDs)
        {

            int n = hitIDs.Count;
            //If list is empty, abort
            if (n == 0)
            {
                return null;
            }


            //If list isn't empty, sort it 
            hitIDs.Sort();

            //Find the max frequency using  
            //Linear traversal 
            int max_count = 1, res = hitIDs[0];
            int curr_count = 1;

            for (int i = 1; i < n; i++)
            {
                if (hitIDs[i] == hitIDs[i - 1])
                    curr_count++;
                else
                {
                    if (curr_count > max_count)
                    {
                        max_count = curr_count;
                        res = hitIDs[i - 1];
                    }
                    curr_count = 1;
                }
            }

            //If last element is most frequent 
            if (curr_count > max_count)
            {
                max_count = curr_count;
                res = hitIDs[n - 1];
            }

            //Return GameObject if present, or null if not
            if (BM.LookUpObjectByID(res) == null)
            {
                return null;
            }
            else
            {
                return BM.LookUpObjectByID(res);
            }


        }

        /// <summary>
        /// Removes locked target object, clears references and adds explosion effect
        /// upon deleting the respective gameobject.
        /// </summary>
        private void RemoveTarget() 
        {
            if (m_LockedTarget != null)
            {
                //Removes all references of the locked target in the scene, so it can be safely removed
                BM.UpdateReferences(m_LockedTarget, m_LockedTarget.GetComponent<InteractableCustom>().GetID());
                Collider[] hits = Physics.OverlapSphere(m_LockedTarget.transform.position, ExplosionRadius);
                foreach (Collider h in hits)
                {
                    GameObject go = h.gameObject;
                    //Only affects other interactables for now
                    if (go.GetComponent<InteractableCustom>())
                    {

                        Rigidbody rb = h.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            rb.AddExplosionForce(ExplosionForce, m_LockedTarget.transform.position, ExplosionRadius, 5.0f);
                        }
                    }
                }

                Destroy(m_LockedTarget);
                m_LockedTarget = null;
            }

        }
    }

}

