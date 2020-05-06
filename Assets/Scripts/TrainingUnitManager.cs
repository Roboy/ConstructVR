using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Construct.Utilities
{
    //Is unique, one per training unit, depends on what content should be taught
    //so e.g. vestUnitManager inherits from TrainingUnitManager
    public class TrainingUnitManager : Singleton<TrainingUnitManager>
    {
        public bool unitGoalComplete = false;
        //Cues, hints the operator is getting
        public List<string> instructions;
    }
}
