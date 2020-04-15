using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bhaptics.Tact.Unity;

namespace Construct.Hugs
{
    public class HugZone : MonoBehaviour
    {
        [Header("Attributes")]
        public bool TriggerTacFeedback = true;
        public string MatchAgainst = "default";
        [SerializeField]
        private bool m_MatchSuccess = false;
        [SerializeField]
        private float m_DistanceToBodyPart = -1.0f;
        [SerializeField]
        private float m_MaxDistance = -1.0f;

        


        private void OnTriggerEnter(Collider other)
        {
            
            if (other.transform.tag == MatchAgainst)
            {
                m_MatchSuccess = true;
                m_MaxDistance = other.transform.position.z - transform.position.z;
                ProcessDistance(other);
            }
            HugManager.Instance.CheckForHug();
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.transform.tag == MatchAgainst) 
            {
                ProcessDistance(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            
            if (other.transform.tag == MatchAgainst)
            {
                m_MatchSuccess = false;
                m_DistanceToBodyPart = -1.0f;
                ProcessDistance(other);
            }
        }

        public bool CheckSuccess()
        {
            return m_MatchSuccess;
        }

        private void ProcessDistance(Collider other)
        {
            //Only continue if TactSource is a file
            if (other.gameObject.GetComponentInChildren<TactSource>().FeedbackType != FeedbackType.TactFile)
            {
                return;
            }

            //Calculate distance between player part and hug zone
            m_DistanceToBodyPart = other.transform.position.z - transform.position.z;
            //Calculate intensity based on distance and min/max values of tactile intensity
            float Intensity = (m_MaxDistance - m_DistanceToBodyPart) * (4.8f / m_MaxDistance) + 0.2f;
            //Change tactile intensity accordingly
            other.gameObject.GetComponentInChildren<TactSource>().Intensity = Intensity;
        }
    }
}