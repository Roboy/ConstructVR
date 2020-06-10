using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace Construct.Interaction
{
    /// <summary>
    /// Controls a game character that interacts with the operator.
    /// This instructor can hand out objects that can be picked up.
    /// It is further able to indicate intentions, e.g. waving, towards the operator.
    /// </summary>
    public class InstructorController : Singleton<InstructorController>
    {

        #region PUBLIC
        [Header("Attributes")]
        public float Health = 100.0f;
        [Range(0.5f, 3.0f)]
        public float HoldingTime;
        [Range(0.5f, 3.0f)]
        public float DropTime;
        public List<GameObject> PrefabItems;
        public GameObject PrefabItem;
        public bool RandomSpawn;
        public GameObject Hand;
        public SteamVR_Action_Boolean PresentItem;
        public SteamVR_Input_Sources handType;
        #endregion

        #region PRIVATE
        private bool m_Walking = false;
        [Header("References")]
        [SerializeField]
        private Animator m_Animator;
        [SerializeField]
        private GameObject HeldItem;
        private bool m_initialised = false;
        #endregion


        void Start()
        {
            Initialise();
        }

        void Update()
        {
            if (!m_initialised)
            {
                Initialise();
                return;
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                if (HeldItem == null)
                {
                    StartCoroutine(PresentObject());
                       
                }
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                ToggleWalk();
            }

        }

        private void Initialise() 
        {
            m_Animator = GetComponent<Animator>();
            PresentItem.AddOnStateDownListener(TriggerDown, handType);
            m_initialised = true;
        }

        /// <summary>
        /// If a certain button is pressed on the controller an object is spawned
        /// and raised to be picked up by the operator.
        /// </summary>
        /// <param name="fromAction"></param>
        /// <param name="fromSource"></param>
        private void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            Debug.Log("Trigger is down");
            if (HeldItem == null)
            {
                RaiseObject();
            }

        }

        private GameObject RandomizeItem() 
        {
            int count = PrefabItems.Count;
            int rand = Random.Range(0, count);
            return PrefabItems[rand];
        }
        /// <summary>
        /// Turns the walking animation of the instructor character on or off.
        /// </summary>
        private void ToggleWalk()
        {
            m_Walking = !m_Walking;
            m_Animator.SetBool("isWalking", m_Walking);
        }

        private IEnumerator PresentObject()
        {
            float CurrentTimer = 0.0f;

            //Raise Arm and attach object to hold
            m_Animator.SetTrigger("RaiseObject");
            SpawnItem();

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

        private void SpawnItem()
        {
            
            GameObject GO;
            if (RandomSpawn) 
            {
                GO = RandomizeItem();
            }
            else
            {
                GO = PrefabItem;
            }

            GameObject Item = GameObject.Instantiate(GO, Hand.transform);
            Item.transform.localPosition = GO.GetComponent<InteractableCustom>().PositionOffset;
            Item.transform.Rotate(GO.GetComponent<InteractableCustom>().RotationOffset, Space.Self);
            HeldItem = Item;

        }

        private void RaiseObject()
        {
            //Raise Arm and attach object to hold
            m_Animator.SetTrigger("RaiseObject");
            SpawnItem();
            m_Animator.SetBool("isHoldingObject", true);

        }

        private void RaiseArm() 
        {
            m_Animator.SetTrigger("RaiseObject");
            m_Animator.SetBool("isHoldingObject", true);
        }

        private void LowerArm() 
        {
            m_Animator.SetBool("isHoldingObject", false);
        }

        /// <summary>
        /// This enables the object to be picked up by the operator.
        /// It detaches the object from its parent (instructor).
        /// </summary>
        private void DetachItem()
        {

            if (HeldItem != null)
            {
                GameObject go = HeldItem;
                HeldItem = null;
                go.transform.parent = null;
                Rigidbody rb = go.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.useGravity = true;
            }

        }

        public void ReceiveTask(int ID)
        {

            switch (ID)
            {
                case 0:
                    LowerArm();
                    break;
                case 1:
                    DetachItem();
                    break;
                default:
                    break;
            }
        }
    }
}
