using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Construct.Utilities
{

    public class InstructionManager : Singleton<InstructionManager>
    {
        [Header("Instructions")]
        public InstructionSet currentInstructionSet;
        public List<Instruction> currentInstructionList;
        public Instruction currentInstruction;
        public string instructionTag;

        /// <summary>
        /// With no arguments it resets all instruction references to null.
        /// </summary>
        public void UpdateReferences()
        {
            currentInstructionSet = null;
            currentInstructionList = null;
            currentInstruction = null;
        }
        /// <summary>
        /// Update instruction references based on latest scene.
        /// </summary>
        /// <param name="scene">Latest loaded scene.</param>
        public void UpdateReferences(Scene scene) 
        {
            if (scene == null) 
            {
                UpdateReferences();
                return;
            }

            GameObject[] objInScene = scene.GetRootGameObjects();
            for (int i = 0; i < objInScene.Length; i++)
            {
                if (objInScene[i].tag == instructionTag)
                {
                    currentInstructionSet = objInScene[i].GetComponent<InstructionSet>();
                    currentInstructionList = currentInstructionSet.Instructions;
                    currentInstruction = currentInstructionList[0];
                    break;
                }

                UpdateReferences();
            }

        }

        public void PlayNextInstruction() 
        {
            if (currentInstructionSet == null) 
            {
                return;
            }

            currentInstruction = currentInstructionSet.LoadNextInstruction();
        }
        public void PlayPreviousInstruction() 
        {
            if (currentInstructionSet == null)
            {
                return;
            }
            currentInstruction = currentInstructionSet.LoadPreviousInstruction();
        }
        public void ReplaytInstruction() 
        {
            if (currentInstructionSet == null)
            {
                return;
            }

            currentInstruction = currentInstructionSet.ReloadCurrentInstruction();
        }


    }

}
