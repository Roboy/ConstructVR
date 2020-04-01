using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRDummyController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Camera m_MainCamera;
    void Start()
    {
       m_MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForGrab();
    }

    private void CheckForGrab() 
    {
        RaycastHit hit;
        Ray ray;
        ray = m_MainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit)) 
        {
            if(hit.transform.GetComponent<Interactable>() != null)
                {
                hit.transform.GetComponent<Interactable>().OnGrab();
            }
        }
    }
}
