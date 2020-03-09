using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ObjectManager replaces BallManager in the future. Extending it, with the possibilities to spawn and despawn
/// object in addition of just holding references.
/// </summary>
public class ObjectManager : MonoBehaviour
{
    [Header("Attributes")]
    public bool Spawning = false;
    [Header("References")]
    public List<GameObject> PrefabObjects = new List<GameObject>();

    public void SpawnObject(GameObject prefab)
    {

    }

    public void DespawnObject(GameObject obj)
    {

    }

    
}
