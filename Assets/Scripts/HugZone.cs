using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bhaptics.Tact.Unity;

namespace Construct.Hugs
{
    /// <summary>
    /// Validates if a player body part is located correctly so that a hug event can take place.
    /// </summary>
    public class HugZone : MonoBehaviour
    {
        #region VAR
        [Header("Attributes")]
        public bool TriggerTacFeedback = true;
        public string MatchAgainst = "default";
        [SerializeField]
        private bool m_MatchSuccess = false;
        [SerializeField]
        private float m_DistanceToBodyPart = -1.0f;
        [SerializeField]
        private float m_MaxDistance = -1.0f;
        #endregion


        /// <summary>
        /// Player body part enters HugZone.
        /// </summary>
        /// <param name="other">The player body part.</param>
        private void OnTriggerEnter(Collider other)
        {
            
            if (other.transform.tag == MatchAgainst)
            {
                m_MatchSuccess = true;
                m_MaxDistance = other.transform.position.z - transform.position.z;
                HugManager.Instance.CheckForHug();
            }
            
        }
        /// <summary>
        /// Player body part resides inside HugZone, triggers calculation of distance and results.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerStay(Collider other)
        {
            if (other.transform.tag == MatchAgainst) 
            {
                ProcessDistance(other);
            }
        }

        /// <summary>
        /// Player body part exits the HugZone.
        /// </summary>
        /// <param name="other">The player body part.</param>
        private void OnTriggerExit(Collider other)
        {
            
            if (other.transform.tag == MatchAgainst)
            {
                m_MatchSuccess = false;
                m_MaxDistance = -1.0f;
                m_DistanceToBodyPart = -1.0f;
            }
        }
        /// <summary>
        /// Returns whether zone is occupied correctly or not.
        /// </summary>
        /// <returns>Success of Zone</returns>
        public bool CheckSuccess()
        {
            return m_MatchSuccess;
        }

        /// <summary>
        /// Calculates intensity of tactile feedback with regard to distance towards touched target.
        /// </summary>
        /// <param name="other">The player body part.</param>
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