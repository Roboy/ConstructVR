using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Effector motion follows the movement of a gameobject.
/// </summary>
public class FollowEffector : MonoBehaviour
{
    public GameObject FollowObject;

    void Update()
    {
        if (FollowObject != null) 
        {
            transform.position = FollowObject.transform.position;
            transform.rotation = FollowObject.transform.rotation;
        }
            
    }
}
