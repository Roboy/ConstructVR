using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ObjectManager replaces BallManager in the future. Extending it, with the possibilities to spawn and despawn
/// objects in addition to just holding references.
/// </summary>
public class ObjectManager : MonoBehaviour
{
    #region VAR
    [Header("Attributes")]
    public bool Spawning = false;
    [Header("References")]
    public List<GameObject> PrefabObjects = new List<GameObject>();
    #endregion

    public void SpawnObject(GameObject prefab)
    {
        //TODO
    }

    public void DespawnObject(GameObject obj)
    {
        //TODO
    }

    
}
