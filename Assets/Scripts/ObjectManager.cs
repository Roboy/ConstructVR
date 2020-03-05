using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
