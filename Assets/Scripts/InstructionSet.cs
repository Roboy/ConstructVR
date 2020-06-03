using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Construct.Utilities
{
    public class InstructionSet : MonoBehaviour
    {
        public List<Instruction> Instructions;

        public Instruction currentInstruction;


        private void Start()
        {
            Initialise();   
        }

        private void Initialise() 
        {
            if (Instructions.Count > 0)
            {
                currentInstruction = Instructions[0];
            }
        }

        public Instruction LoadNextInstruction() 
        {
            currentInstruction =  DataStructHelper.nextElementInList<Instruction>(Instructions, currentInstruction);
            return currentInstruction;
        }
        public Instruction LoadPreviousInstruction() 
        {
            currentInstruction = DataStructHelper.previousElementInList<Instruction>(Instructions, currentInstruction);
            return currentInstruction;
        }
        public Instruction ReloadCurrentInstruction() 
        {
            //Obsolete, TODO replay/display again instruction
            //currentInstruction = Instructions[0];
            return currentInstruction;
        }

        public void ResetInstructions() 
        {
            currentInstruction = Instructions[0];
        }

    }

}
