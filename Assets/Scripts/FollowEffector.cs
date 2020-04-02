using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEffector : MonoBehaviour
{
    public GameObject FollowObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (FollowObject != null) 
        {
            transform.position = FollowObject.transform.position;
            transform.rotation = FollowObject.transform.rotation;
        }
            
    }
}
