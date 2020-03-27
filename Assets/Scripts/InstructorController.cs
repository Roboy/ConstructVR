using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructorController : MonoBehaviour
{

    #region PUBLIC
    [Header("Attributes")]
    public float Health = 100.0f;
    [Range(0.5f, 3.0f)]
    public float HoldingTime;
    [Range(0.5f, 3.0f)]
    public float DropTime;
    public GameObject Item;
    public GameObject Hand;
    public Vector3 Offset;
    #endregion

    #region PRIVATE
    private bool m_Walking = false;
    [Header("References")]
    [SerializeField]
    private Animator m_Animator;
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
            ToggleWalk();    
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
           StartCoroutine(PresentObject(Item));
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnBall();
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
        SpawnBall();

        //Holding Object in place for a period of time, before dropping it
        while (CurrentTimer < HoldingTime)
        {
            CurrentTimer += Time.deltaTime;
            m_Animator.SetBool("isHoldingObject", true);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        //Lower Arm and initialize drop
        m_Animator.SetBool("isHoldingObject", false);
        StartCoroutine(DetachBall());

        yield return null;

    }

    void SpawnBall() 
    {
        GameObject Ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Ball.transform.localScale = new Vector3(.1f, .1f, .1f);
        Ball.transform.parent = Hand.transform;
        Ball.transform.localPosition = Offset;
        //Ball.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        Ball.transform.GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        Item = Ball;
    
    }


    private IEnumerator DetachBall()
    {

        if(Item != null) 
        {
            float currentTimer = 0.0f;
            GameObject go = Item;
            Item = null;
            go.transform.parent = null;
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.useGravity = true;
            //Collider col = Item.AddComponent<SphereCollider>();

            //while(currentTimer < DropTime) 
            //{
            //    currentTimer += Time.deltaTime;
            //    yield return new WaitForSeconds(Time.deltaTime);
            //}

            //Destroy(go);
        }

        yield return null;
    }
}
