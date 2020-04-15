using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Construct.Hugs
{
    public class HugManager : Singleton<HugManager>
    {
        public List<HugZone> HugZones;

        [SerializeField]
        private bool m_Hugged = false;

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
