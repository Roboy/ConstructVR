using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Construct.Utilities
{
    public class TrainingManager : Singleton<TrainingManager>
    {
        [Header("Attributes")]
        public bool operatorCalibrated = false;
        //used to avoid nullpointers, e.g. when roboy is reference training objects
        public bool trainingUnitSwitching = false;
        public bool currentUnitCompleted = false;
        [Header("References")]
        public GameObject prefab_RoboyModel;
        public GameObject prefab_BulletBridge;
        [Header("Units")]
        public string currentTitle;
        public TrainingUnit currentTrainingUnit;
        public List<TrainingUnit> trainingUnits;

        private void Start()
        {
            if (trainingUnits.Count > 0) 
            {
                currentTrainingUnit = trainingUnits[0];
                currentTitle = currentTrainingUnit.title;
            }
        }

        private void Update()
        {
            if (!operatorCalibrated)
            {
                CalibrateOperator();
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                LoadPreviousTrainingUnit();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                LoadNextTrainingUnit();
            }
        }

        private void CalibrateOperator()
        {
            //Do calibration
        }

        private void LoadNextTrainingUnit() 
        {
            trainingUnitSwitching = true;
            currentUnitCompleted = false;
            int currentUnitID = trainingUnits.IndexOf(currentTrainingUnit);
            int nextUnitID = -1;

            if (currentUnitID == trainingUnits.Count - 1) 
            {
                nextUnitID = 0;
            }
            else 
            {
                nextUnitID = currentUnitID + 1;
            }

            currentTrainingUnit = trainingUnits[nextUnitID];
            currentTitle = currentTrainingUnit.title;
            Debug.Log(nextUnitID);
            trainingUnitSwitching = false;
        }

        private void LoadPreviousTrainingUnit() 
        {
            trainingUnitSwitching = true;
            currentUnitCompleted = false;
            int currentUnitID = trainingUnits.IndexOf(currentTrainingUnit);
            int nextUnitID = -1;

            if (currentUnitID == 0)
            {
                nextUnitID = trainingUnits.Count - 1;
            }
            else
            {
                nextUnitID = currentUnitID - 1;
            }

            currentTrainingUnit = trainingUnits[nextUnitID];
            currentTitle = currentTrainingUnit.title;
            Debug.Log(nextUnitID);
            trainingUnitSwitching = false;
        }

        private void RestartCurrentTrainingUnit()
        {
            //Restart this unit
            trainingUnitSwitching = true;
            currentUnitCompleted = false;
        }

        private void SpawnTrainingObjects() 
        {
            //Spawn Objects at offsets regarding origin point
            //Register spawned objects with manager
        }

        private void DeSpawnTrainingObjects()
        {
            //Remove references (update references)
            //Destroy GameObjects (spawned objects)
        }
    }
}
