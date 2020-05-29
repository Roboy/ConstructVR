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

    [CreateAssetMenu(fileName = "Instruction", menuName = "Training/Instruction", order = 1)]
    public class Instruction : ScriptableObject
    {
        public InstructionType type;

        [ConditionalField("type", false, InstructionType.TXT), TextArea]
        public string text;
        [ConditionalField("type", false, InstructionType.PIC)]
        public Texture picture;
        [ConditionalField("type", false, InstructionType.AUD)]
        public AudioClip audio;
        [ConditionalField("type", false, InstructionType.VID)]
        public Object video;
    }
}