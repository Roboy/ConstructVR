using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void InitializeBalls()
    {
        int id = 1;
        //Search through all children
        foreach (Transform child in transform)
        {
            //If it is interactable, add it to the list
            if (child.GetComponent<Interactable>())
            {
                child.GetComponent<Interactable>().SetID(id);
                m_Balls.Add(child.gameObject);
                m_InteractableObjects.Add(id, child.gameObject);
            }

            id++;
        }
    }

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

    public void UpdateReferences(GameObject go, int ID)
    {
        m_Balls.Remove(go);
        m_InteractableObjects.Remove(ID);
    }
}
