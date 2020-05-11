using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        [Header("Scenes")]
        public List<SceneAsset> availableScenes;
        public SceneAsset SceneToLoad;
        public SceneAsset LatestAddedScene;
        public List<SceneAsset> loadedScenes;
        [Header("Files")]
        public string scenePath = "Assets/Scenes/";
        public string sceneExtension = ".unity";
        [SerializeField]
        private bool SceneLock = false;


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
                //LoadPreviousTrainingUnit();
                StartCoroutine(UnloadScene(SceneToLoad));
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                //LoadNextTrainingUnit();
                StartCoroutine(LoadScene(SceneToLoad));
            }

            if (Input.GetKeyDown(KeyCode.K))
            {

            }
        }

        private void CalibrateOperator()
        {
            //Do calibration
        }
        #region TrainingUnits
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
        #endregion

        #region SceneManagement
        IEnumerator LoadScene(SceneAsset scene)
        {
            

            if (CheckSceneLoadingStatus(scene))
            {
                Debug.Log(scene.name + " is already loaded!");
                yield break;
            }

            //Needs SceneLock to execute further
            if (!CheckForSceneLock())
            {
                yield break;
            }

            string pathToSceneAsset = scenePath + scene.name + sceneExtension;
            int indexSceneToLoad = SceneUtility.GetBuildIndexByScenePath(pathToSceneAsset);
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(indexSceneToLoad, LoadSceneMode.Additive);
            //Once loading is complete, call delegate function
            asyncOperation.completed += delegate (AsyncOperation op)
            {
                OnSceneLoaded(scene);
            };

        }

        IEnumerator UnloadScene(SceneAsset scene)
        {
            if (!CheckSceneLoadingStatus(scene))
            {
                Debug.Log(scene.name + " is already unloaded!");
                yield break;
            }

            //Needs SceneLock to execute further
            if (!CheckForSceneLock())
            {
                yield break;
            }

            string pathToSceneAsset = scenePath + scene.name + sceneExtension;
            int indexSceneToUnload = SceneUtility.GetBuildIndexByScenePath(pathToSceneAsset);
            AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(indexSceneToUnload, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            //Once unloading is complete, call delegate function
            asyncOperation.completed += delegate (AsyncOperation op)
            {
                OnSceneUnloaded(scene);
            };


        }


        /// <summary>
        /// Checks if a specific scene is already active in hierachy.
        /// </summary>
        /// <param name="scene">Scene to check.</param>
        /// <returns>Returns true if scene is already loaded, false if not.</returns>
        private bool CheckSceneLoadingStatus(SceneAsset scene)
        {
            bool result = false;
            string sceneName = scene.name;

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).name == sceneName)
                {
                    result = true;
                }
            }

            return result;
        }

        private void OnSceneLoaded(SceneAsset scene)
        {
            loadedScenes.Add(scene);
            UpdateLatestSceneEntry();
            Debug.Log(scene.name + " has been successfully loaded!");
            SceneLock = false;

        }

        private void OnSceneUnloaded(SceneAsset scene)
        {
            loadedScenes.Remove(scene);
            UpdateLatestSceneEntry();
            Debug.Log(scene.name + " has been successfully unloaded!");
            SceneLock = false;
        }

        private void UpdateLatestSceneEntry() 
        {
            int count = SceneManager.sceneCount;
            if (count == 1) 
            {
                LatestAddedScene = null;
                return;
            }

            Scene latest = SceneManager.GetSceneAt(count - 1);
            LatestAddedScene = availableScenes.Find(x => x.name == latest.name);
        }

        private bool CheckForSceneLock() 
        {
            if (SceneLock) 
            {
                return false;
            }
            else 
            {
                SceneLock = true;
                return true;
            }
        }

        #endregion
    }
}
