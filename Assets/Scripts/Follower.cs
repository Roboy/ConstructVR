using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class Follower : MonoBehaviour
{
    #region PUB VAR
    //Toggle autopilot ON/OFF
    [Header ("Attributes")]
    public bool isAutoPiloting = false;
    //Speed of travel
    public float FollowSpeed = 5.0f;

    //Path to follow 
    [Header("References")]
    public PathCreator PathCreator;

    #endregion

    #region PRV VAR
    private float m_DistanceTravelled;
    private Quaternion m_InitRotation;
    [SerializeField]
    private Animator m_Animator;
    private bool m_LoadingComplete = false;
    #endregion

    private void Awake()
    {
        m_InitRotation = transform.rotation;
        m_Animator = GetComponent<Animator>();
        m_LoadingComplete = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAutoPiloting)
        {
            AutoPilot();
        }

    }

    public void AutoPilot()
    {
        m_DistanceTravelled += FollowSpeed * Time.deltaTime;
        transform.position = PathCreator.path.GetPointAtDistance(m_DistanceTravelled);
        transform.rotation = PathCreator.path.GetRotationAtDistance(m_DistanceTravelled);
    }
    public void DisableAutoPilot()
    {
        transform.rotation = new Quaternion(m_InitRotation.x, transform.rotation.y, m_InitRotation.z, transform.rotation.w);
    }

    private void OnValidate()
    {
        if (m_LoadingComplete)
        {

            if (isAutoPiloting)
            {
                //Enable Root Motion
                m_Animator.applyRootMotion = true;
            }

            if (!isAutoPiloting)
            {   
                DisableAutoPilot();
            }
        }
    }

}
