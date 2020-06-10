using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Maps movement of tracked object towards a avatar body part. 
/// </summary>
[System.Serializable]
public class VRMap 
{
    public Transform VRTarget;
    public Transform RigTarget;
    public Vector3 TrackingPositionOffset;
    public Vector3 TrackingRotationOffset;

    public void Map() 
    {
        RigTarget.position = VRTarget.TransformPoint(TrackingPositionOffset);
        RigTarget.rotation = VRTarget.rotation * Quaternion.Euler(TrackingRotationOffset);
    }
}
public class VRRig : MonoBehaviour
{
    [Header("VRMaps")]
    public VRMap Head;
    public VRMap LeftHand;
    public VRMap RightHand;

    [Header("Constraints")]
    public Transform HeadConstraint;
    public Vector3 HeadBodyOffset;

    
    void Start()
    {
        HeadBodyOffset = transform.position - HeadConstraint.position;
    }

    
    void LateUpdate()
    {
        transform.position = HeadConstraint.position + HeadBodyOffset;
        transform.forward = Vector3.ProjectOnPlane(HeadConstraint.forward, Vector3.up).normalized;

        Head.Map();
        LeftHand.Map();
        RightHand.Map();
    }
}
