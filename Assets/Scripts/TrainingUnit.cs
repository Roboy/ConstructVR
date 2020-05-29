using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Construct.Utilities
{
    [CreateAssetMenu(fileName = "Unit", menuName = "Training/Unit", order = 2)]
    public class TrainingUnit : ScriptableObject
    {
        public string title;
        public Vector3 spawnpoint;
        public List<GameObject> trainingObjects;
    }
}
