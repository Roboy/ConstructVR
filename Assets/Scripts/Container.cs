using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Will soon be omitted, replaced by ObjectManager
/// </summary>
public class Container : MonoBehaviour
{
    [Header("Attributes")]
    [Range(50.0f, 850.0f)]
    public float ExplosionForce = 200.0f;
    [Range(0.5f, 5.0f)]
    public float ExplosionRadius = 4.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, ExplosionRadius);
            foreach (Collider h in hits)
            {
                GameObject go = h.gameObject;
                if (go.GetComponent<Interactable>())
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, ExplosionRadius);

    }
}
