using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Construct.Hugs
{
    /// <summary>
    /// Evaluates necessary HugZones for validity so that a hug event can take place.
    /// </summary>
    public class HugManager : Singleton<HugManager>
    {
        public List<HugZone> HugZones;

        [SerializeField]
        private bool m_Hugged = false;

        /// <summary>
        /// If all single HugZones are valid, a hug event is possible.
        /// </summary>
        /// <returns>Hug or no hug, that's the question.</returns>
        public bool CheckForHug()
        {
            bool result = false;

            //No hug zones, no check needed.
            if (HugZones.Count == 0)
            {
                return result;
            }
            //Compare validity of all zones, only holds if ALL are valid
            else
            {
                result = HugZones[0].CheckSuccess();

                foreach (HugZone h in HugZones)
                {
                    result = result && h.CheckSuccess();
                }
            }

            //TODO
            //If all zones are valid ->trigger successful hug
            Debug.Log("All zones valid? -> " + result);
            m_Hugged = result;
            return result;

        }
    }
}
