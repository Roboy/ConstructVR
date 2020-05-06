using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Construct.Utilities
{
    [CreateAssetMenu(fileName = "Unit", menuName = "ScriptableObjects/Training", order = 1)]
    public class TrainingUnit : ScriptableObject
    {
        public string title;
        public Vector3 spawnpoint;
        public List<GameObject> trainingObjects;
    }
}
