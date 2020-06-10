using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Construct.Utilities
{

    public enum InstructionType
    {
        TXT = 0,
        PIC = 1,
        AUD = 2,
        VID = 3
    }
    /// <summary>
    /// An instruction is an aid for the operator to learn about a certain topic. This can either
    /// be a simple text, a picture, an audio file or a video file.
    /// </summary>
    [CreateAssetMenu(fileName = "Instruction", menuName = "Training/Instruction", order = 1)]
    public class Instruction : ScriptableObject
    {
        public InstructionType type;

        //Conditional fields hide/show properties depending on a bool value.
        //For instance if the instruction is of the type text, all other properties
        //like picture, audio and video are hidden in the inspector.
        [ConditionalField("type", false, InstructionType.TXT), TextArea]
        public string text;
        [ConditionalField("type", false, InstructionType.PIC)]
        public Sprite picture;
        [ConditionalField("type", false, InstructionType.AUD)]
        public AudioClip audio;
        [ConditionalField("type", false, InstructionType.VID)]
        public Object video;
    }
}