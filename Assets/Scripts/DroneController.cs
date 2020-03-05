using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class DroneController : MonoBehaviour
{

    #region PUB VAR
    //Toggle autopilot ON/OFF
    [Header("Attributes")]
    //Speed of AutoPilot mode
    public float FollowSpeed = 5.0f;
    //Speed of Manual mode
    public float HorsePower = 1.0f;
    public bool AutoPilotFeature = false;
    public int Counter = 0;
    

    //Path to follow 
    [Header("References")]
    public PathCreator PathCreator;

    #endregion

    #region PRV VAR
    //Distance of drone so far
    private float m_DistanceTravelled;
    //Rotation of drone @zero
    private Quaternion m_InitRotation;
    //Needed for OnValidate
    private bool m_LoadingComplete = false;
    private bool m_RotationIncomplete = false;

    [SerializeField]
    private Transform m_Drone;
    private Vector3 m_DronePosition;

    [SerializeField]
    private Animator m_DroneAnimator;

    private bool m_TiltingLeft;
    private bool m_TiltingRight;
    private bool m_TiltingForward;
    private bool m_TiltingBackward;

    [Header ("Live Input")]
    [SerializeField]
    private float m_X;
    [SerializeField]
    private float m_Z;

    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        m_Drone = GetComponent<Transform>();
        m_DroneAnimator = GetComponent<Animator>();
        m_InitRotation = transform.rotation;
        m_LoadingComplete = true;

        //Set drone status to idle
        m_TiltingLeft = false;
        m_TiltingRight = false;
        m_TiltingForward = false;
        m_TiltingBackward = false;
    }

    void Update()
    {
        //Switch between modes
        if (Input.GetKeyDown(KeyCode.I))
        {
            HandleModeSwitch();  
        }

        //Autonomous flight, following path
        if (AutoPilotFeature)
        {
            HandleAutoPilot();
        }

        //Manual flight, at own risk
        if (!AutoPilotFeature)
        {
            HandleManualInput();
        }
    }

    private void LateUpdate()
    {
        if (m_RotationIncomplete)
        {
            Counter += 1;
            Debug.Log("Late Update @" + Time.fixedTime + "/" + Counter);
            transform.rotation = new Quaternion(m_InitRotation.x, transform.rotation.y, m_InitRotation.z, transform.rotation.w);
            m_DroneAnimator.applyRootMotion = false;
            m_RotationIncomplete = false;

        }
    }

    //Called when variable in editor has changed
    private void OnValidate()
    {
        if (m_LoadingComplete)
        {
        //AutoPilot On

        //AutoPilot OFF

        }
    }

    /// <summary>
    /// Controlling the drone via keyboard (WASD, arrows)
    /// </summary>
    void HandleManualInput()
    {
        //Animate Drone
        m_X = Input.GetAxis("Horizontal");
        m_Z = Input.GetAxis("Vertical");
        m_DroneAnimator.SetFloat("X", m_X);
        m_DroneAnimator.SetFloat("Z", m_Z);

        //Move Drone
        m_Drone.position = new Vector3(m_Drone.position.x - m_X * HorsePower * Time.deltaTime,
                           m_Drone.position.y, m_Drone.position.z - m_Z * HorsePower * Time.deltaTime);
    }

    /// <summary>
    /// Automatically moves the drone on a fixed path
    /// </summary>
    void HandleAutoPilot()
    {
        m_DistanceTravelled -= FollowSpeed * Time.deltaTime;
        transform.position = PathCreator.path.GetPointAtDistance(m_DistanceTravelled);
        transform.rotation = PathCreator.path.GetRotationAtDistance(m_DistanceTravelled);
    }

    /// <summary>
    /// Toggle between, auto and manual mode
    /// </summary>
    void HandleModeSwitch()
    {
        if (m_LoadingComplete)
        {
            AutoPilotFeature = !AutoPilotFeature;
            if (AutoPilotFeature)
            {
                m_DroneAnimator.applyRootMotion = true;
                
            }
            if (!AutoPilotFeature)
            {
                Counter += 1;
                Debug.Log("Switch @" + Time.fixedTime + "/" + Counter);
                
                m_RotationIncomplete = true;

            }
        }
    }

    /// <summary>
    /// Animates drone due to arrow-key input, bad, do not use it.
    /// </summary>
    void HandleInputBadly()
    {
        m_DronePosition = m_Drone.transform.position;

        //Moving Left
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            m_DroneAnimator.SetBool("TiltingLeft", true);
            m_TiltingLeft = true;
            m_Drone.transform.position = new Vector3(m_DronePosition.x + HorsePower * Time.deltaTime, m_DronePosition.y, m_DronePosition.z);
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            m_DroneAnimator.SetBool("TiltingLeft", false);
            m_TiltingLeft = false;
        }

        //Moving Right
        if (Input.GetKey(KeyCode.RightArrow))
        {
            m_DroneAnimator.SetBool("TiltingRight", true);
            m_TiltingRight = true;
            m_Drone.transform.position = new Vector3(m_DronePosition.x - HorsePower * Time.deltaTime, m_DronePosition.y, m_DronePosition.z);
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            m_DroneAnimator.SetBool("TiltingRight", false);
            m_TiltingRight = false;
        }

        //Moving Forward
        if (Input.GetKey(KeyCode.UpArrow))
        {
            m_DroneAnimator.SetBool("TiltingForward", true);
            m_TiltingForward = true;
            m_Drone.transform.position = new Vector3(m_DronePosition.x, m_DronePosition.y, m_DronePosition.z - HorsePower * Time.deltaTime);
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            m_DroneAnimator.SetBool("TiltingForward", false);
            m_TiltingForward = false;
        }

        //Moving Backward
        if (Input.GetKey(KeyCode.DownArrow))
        {
            m_DroneAnimator.SetBool("TiltingBackward", true);
            m_TiltingBackward = true;
            m_Drone.transform.position = new Vector3(m_DronePosition.x, m_DronePosition.y, m_DronePosition.z + HorsePower * Time.deltaTime);
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            m_DroneAnimator.SetBool("TiltingBackward", false);
            m_TiltingBackward = false;
        }

    }
}
