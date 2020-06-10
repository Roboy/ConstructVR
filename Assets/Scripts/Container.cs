using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Construct.Interaction
{
    /// <summary>
    /// Will soon be omitted, replaced by ObjectManager
    /// Manages ball objects, can create explosion inbetween balls
    /// </summary>
    ///
    public class Container : MonoBehaviour
    {
        #region VAR
        [Header("Attributes")]
        [Range(50.0f, 850.0f)]
        public float ExplosionForce = 200.0f;
        [Range(0.5f, 5.0f)]
        public float ExplosionRadius = 4.0f;
        #endregion

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                CreateExplosion();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, ExplosionRadius);

        }

        private void CreateExplosion() 
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, ExplosionRadius);
            foreach (Collider h in hits)
            {
                GameObject go = h.gameObject;
                if (go.GetComponent<InteractableCustom>())
                {

                    Rigidbody rb = h.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius, 5.0f);
                    }
                }
            }
        }
    }

}
