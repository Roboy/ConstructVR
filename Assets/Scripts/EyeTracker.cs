using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeTracker : MonoBehaviour
{
    #region VAR

    [Header("Attributes")]
    [SerializeField]
    private Vector2 m_MousePosition;

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
        m_MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    
    void Update()
    {
        m_MousePosition = Input.mousePosition;

        if (m_LookingForTarget == false)
        {
            m_LookingForTarget = true;
            StartCoroutine(CheckForTarget());
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (m_LockedTarget != null)
            {
                BM.UpdateReferences(m_LockedTarget, m_LockedTarget.GetComponent<Interactable>().GetID());
                Destroy(m_LockedTarget);
                m_LockedTarget = null;
            }
                
        }

    }


    /// <summary>
    /// Identifies the object the user is looking at. Weights a number of raycasts in a period of time and identifies the most frequent object.
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckForTarget()
    {
        //Debug.Log("START");
        //Variables
        float Timer = 0.4f;
        float timeStep = 0.1f;
        float currentTime = 0.0f;

        RaycastHit hit;
        Ray ray;
        List<int> hitIDs = new List<int>();

        //Checking at what objects the user looks in a period of time
        while(currentTime < Timer)
        {
            currentTime += timeStep;
            ray = m_MainCamera.ScreenPointToRay(m_MousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;

                if (hit.transform.GetComponent<Interactable>() != null)
                {
                    int id = hit.transform.GetComponent<Interactable>().GetID();
                    hitIDs.Add(id);
                }

            }

            yield return new WaitForSeconds(timeStep);
        }

        //Set target to most looked at object, null if user looks at non interactable objects
        m_LockedTarget = CheckForMostFrequent(hitIDs);
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
}

