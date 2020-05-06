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
        public List<GameObject> anchoredObjects;
        [Header("Units")]
        public string currentTitle;
        public TrainingUnit currentTrainingUnit;
        public List<TrainingUnit> trainingUnits;
        [SerializeField]
        private bool ObjectLock = false;

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
                //return;
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
            ObjectLock = true;
            DeSpawnTrainingObjects();
            SpawnTrainingObjects();
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
            ObjectLock = true;
            DeSpawnTrainingObjects();
            SpawnTrainingObjects();
            trainingUnitSwitching = false;
        }

        private void RestartCurrentTrainingUnit()
        {
            //Restart this unit
            trainingUnitSwitching = true;
            currentUnitCompleted = false;
        }

        /// <summary>
        /// Spawn new training objects, register as anchored objects.
        /// </summary>
        private void SpawnTrainingObjects() 
        {
            //wait for despawn to complete, i.e. despawn sets lock to false
            while (ObjectLock) 
            {
                Debug.Log("Spawner is waiting for lock..");
            }

            //Let the spawning begin, objects are parented to an anchor
            GameObject anchor = new GameObject("Anchor/" + currentTitle);
            anchor.transform.position = currentTrainingUnit.spawnpoint;
            anchoredObjects.Add(anchor);

            //Spawn and register training objects
            List<GameObject> ObjectsToSpawn = currentTrainingUnit.trainingObjects;
            if (ObjectsToSpawn.Count > 0) 
            {
                foreach (GameObject go in ObjectsToSpawn)
                {
                    GameObject obj = Instantiate(go, anchor.transform);
                    anchoredObjects.Add(obj);
                }
            }
        }
        /// <summary>
        /// Destroy anchored Objects, update references.
        /// </summary>
        private void DeSpawnTrainingObjects()
        {
            foreach (GameObject obj in anchoredObjects) 
            {
                Destroy(obj);
            }

            //Flush list of anchored objects
            anchoredObjects.Clear();
            //TODO use to lists to determine which items to keep and which not
            //use property like keepit or flushit

            ObjectLock = false;
        }
    }
}
