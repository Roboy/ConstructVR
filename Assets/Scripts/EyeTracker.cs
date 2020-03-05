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

    // Start is called before the first frame update
    void Start()
    {
        m_MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        //InvokeRepeating("CheckForTarget", 2.0f, 0.5f);
    }

    // Update is called once per frame
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

    void LockOnTarget()
    {

    }

    /// <summary>
    /// Identifies the object the user is looking at. Weights a number of raycasts in a period of time and identifies the most frequent object.
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckForTarget()
    {
        //Debug.Log("START");
        //Variables
        float Timer = 0.5f;
        float timeStep = 0.1f;
        float currentTime = 0.0f;

        RaycastHit hit;
        Ray ray;
        List<int> hitIDs = new List<int>();

        //Checking if in a period of time, user looks at the same target
        while(currentTime < Timer)
        {
            currentTime += timeStep;
            ray = m_MainCamera.ScreenPointToRay(m_MousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                //Debug.Log("Hit @ " + objectHit);
                if (hit.transform.GetComponent<Interactable>() != null)
                {
                    int id = hit.transform.GetComponent<Interactable>().GetID();
                    hitIDs.Add(id);
                }
                //m_LockedTarget = hit.transform;
            }
            //Debug.Log("LOOPING @ "+ currentTime);
            yield return new WaitForSeconds(timeStep);
        }

        //Debug.Log("EXIT");
        CheckForMostFrequent(hitIDs);
        m_LookingForTarget = false;
        yield return null;
    }

    /// <summary>
    /// Samples through hit objects in time period and calculates the most frequent one
    /// </summary>
    private void CheckForMostFrequent(List<int> hitIDs)
    {
        
        int n = hitIDs.Count;
        //If list is empty, abort
        if (n == 0)
        {
            return;
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

        //Set lockedtarget to gameobject with id that occurs the most in time t
        //Debug.Log(res);
        if (BM.LookUpObjectByID(res) == null)
        {
            m_LockedTarget = null;
            return;
        }
        //Debug.Log("MostFrequent: " + BM.LookUpObjectByID(res).name);
        m_LockedTarget = BM.LookUpObjectByID(res);


    }
}

