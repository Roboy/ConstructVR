using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Video;

namespace Construct.Utilities
{
    /// <summary>
    /// Manages the loading/reloading of instructions.
    /// Instructions guide the operator through training.
    /// </summary>
    public class InstructionManager : Singleton<InstructionManager>
    {
        [Header("Instructions")]
        public InstructionSet currentInstructionSet;
        public List<Instruction> currentInstructionList;
        public Instruction currentInstruction;
        public string instructionTag;

        [Header("References")]
        public UnityEngine.UI.Image ImagePlane;
        public TextMeshProUGUI TextField;
        public AudioSource AudioPlayer;
        public VideoPlayer VideoPlayer;
        public Material RenderTextureMaterial;
        public Sprite AudioPlayingSymbol;

        private void Start()
        {
            CleanInstructionPanels();
        }

        private void Update()
        {
            //Play previous instruction
            if (Input.GetKeyDown(KeyCode.Q)) 
            {
                PlayPreviousInstruction();
            }

            //Play next instruction
            if (Input.GetKeyDown(KeyCode.E))
            {
                PlayNextInstruction();
            }
        }

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

            SetInstructionPanels(currentInstruction);
        }

        public void PlayNextInstruction() 
        {
            if (currentInstructionSet == null) 
            {
                return;
            }

            currentInstruction = currentInstructionSet.LoadNextInstruction();
            CleanInstructionPanels();
            SetInstructionPanels(currentInstruction);
            
        }
        public void PlayPreviousInstruction() 
        {
            if (currentInstructionSet == null)
            {
                return;
            }

            currentInstruction = currentInstructionSet.LoadPreviousInstruction();
            CleanInstructionPanels();
            SetInstructionPanels(currentInstruction);
        }
        public void ReplaytInstruction() 
        {
            if (currentInstructionSet == null)
            {
                return;
            }

            currentInstruction = currentInstructionSet.ReloadCurrentInstruction();
            CleanInstructionPanels();
            SetInstructionPanels(currentInstruction);
        }

        /// <summary>
        /// Resets all instruction interfaces, stops playback of instruction media.
        /// </summary>
        public void CleanInstructionPanels() 
        {
            //Reset Text
            TextField.text = "";
            //Reset Image
            ImagePlane.sprite = null;
            //Reset Audio
            AudioPlayer.Stop();
            AudioPlayer.clip = null;
            //Reset Video
            ImagePlane.material = null;
            VideoPlayer.Stop();
            VideoPlayer.clip = null;
        }

        /// <summary>
        /// Fills intruction interfaces based on type of instruction, starts playback where necessary.
        /// </summary>
        /// <param name="input">Instruction to be displayed or media to be played.</param>
        public void SetInstructionPanels(Instruction input) 
        {
            if (input == null)
                return;

            InstructionType InputType = input.type;

            if (InputType == InstructionType.TXT) 
            {
                TextField.text = input.text;
                return;
            }
            if (InputType == InstructionType.PIC)
            {
                ImagePlane.sprite = input.picture;
                return;
            }
            if (InputType == InstructionType.AUD)
            {
                ImagePlane.sprite = AudioPlayingSymbol;
                AudioPlayer.clip = input.audio;
                AudioPlayer.Play();
            }
            if (InputType == InstructionType.VID)
            {
                ImagePlane.material = RenderTextureMaterial;
                VideoPlayer.clip = input.video as VideoClip;
                VideoPlayer.Play();
            }

        }

        public void ResetInstructions() 
        {
            if (currentInstructionSet != null)
            {
                currentInstructionSet.ResetInstructions();
            }
        }


    }

}
