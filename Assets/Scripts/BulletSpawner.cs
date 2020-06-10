using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Can shoot projectiles from a origin position towards a target position.
/// Projectile are influenced by their mass and gravity.
/// Custom gravity values other than g can also be set.
/// </summary>
public class BulletSpawner : MonoBehaviour
{
    #region VAR
    [Header("Attributes")]
    public Vector3 Size = new Vector3(0.5f, 0.5f, 0.5f);
    public float Mass = 1.0f;
    public float Gravity = -9.81f;

    [Header("References")]
    public Transform Origin;
    public Transform Target;

    [SerializeField]
    private List<GameObject> Bullets = new List<GameObject>();
    #endregion


    void Start()
    {
        //Define custom gravity value, default value is g
        Physics.gravity = new Vector3(0.0f, Gravity, 0.0f);   
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) 
        {
            SpawnBullet();
        }

    }

    private void OnValidate()
    {
        Physics.gravity = new Vector3(0.0f, Gravity, 0.0f);
    }

    /// <summary>
    /// Spawns a primitive object and shoots it towards target position.
    /// </summary>
    private void SpawnBullet() 
    {
        //Calculate vector to target object
        Vector3 direction = Target.position - Origin.position;
        direction = direction.normalized;
        //Spawn bullet and initialize it
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.transform.localScale = Size;
        go.transform.position = Origin.position;
        go.transform.forward = direction;
        //Add rigibody component and set it's mass
        Rigidbody rb = go.AddComponent<Rigidbody>();
        rb.mass = Mass;
        //Apply force in direction to target object
        rb.AddForce(direction, ForceMode.Impulse);
        Bullets.Add(go);
    }

    //Destroys projectiles after the reach a certain distance or collide with other objects.
    private void UpdateBullets() 
    {
        if (Bullets.Count > 0) 
        {
        foreach (GameObject go in Bullets) 
            {
                //TODO: Collision check and destroy
            }
        }
    
    }
}
