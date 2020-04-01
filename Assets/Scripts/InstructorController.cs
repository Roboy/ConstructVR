using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructorController : Singleton<InstructorController>
{

    #region PUBLIC
    [Header("Attributes")]
    public float Health = 100.0f;
    [Range(0.5f, 3.0f)]
    public float HoldingTime;
    [Range(0.5f, 3.0f)]
    public float DropTime;
    public GameObject PrefabItem;
    public GameObject Hand;
    public Vector3 Offset;
    #endregion

    #region PRIVATE
    private bool m_Walking = false;
    [Header("References")]
    [SerializeField]
    private Animator m_Animator;
    [SerializeField]
    private GameObject HeldItem;
    #endregion

    
    void Start()
    {
        m_Animator = GetComponent<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) 
        {
            if (HeldItem == null)
            {
                RaiseObject(PrefabItem);
            }  
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
           StartCoroutine(PresentObject(PrefabItem));
        }

    }

    void ToggleWalk() 
    {
        m_Walking = !m_Walking;
        m_Animator.SetBool("isWalking", m_Walking);
    }

    private IEnumerator PresentObject(GameObject go) 
    {
        float CurrentTimer = 0.0f;

        //Raise Arm and attach object to hold
        m_Animator.SetTrigger("RaiseObject");
        SpawnItem(go);

        //Holding Object in place for a period of time, before dropping it
        while (CurrentTimer < HoldingTime)
        {
            CurrentTimer += Time.deltaTime;
            m_Animator.SetBool("isHoldingObject", true);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        //Lower Arm and initialize drop
        m_Animator.SetBool("isHoldingObject", false);
        DetachItem();

        yield return null;

    }

    void SpawnItem(GameObject go) 
    {
        GameObject Item = GameObject.Instantiate(go, Hand.transform);
        Item.transform.localPosition = go.GetComponent<Interactable>().PositionOffset;
        Item.transform.Rotate(go.GetComponent<Interactable>().RotationOffset, Space.Self);
        HeldItem = Item;
    
    }

    private void RaiseObject(GameObject go) 
    {
        //Raise Arm and attach object to hold
        m_Animator.SetTrigger("RaiseObject");
        SpawnItem(go);
        m_Animator.SetBool("isHoldingObject", true);

    }

    public void DetachItem()
    {

        if(HeldItem != null) 
        {
            GameObject go = HeldItem;
            HeldItem = null;
            go.transform.parent = null;
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.useGravity = true;
        }
        m_Animator.SetBool("isHoldingObject", false);

    }
}
