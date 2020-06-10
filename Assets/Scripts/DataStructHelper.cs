using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace Construct.Utilities
{
    /// <summary>
    /// Helps traversing and using data structures such as lists, arrays, etc.
    /// </summary>
    public class DataStructHelper : MonoBehaviour
    {
        #region Lists
        /// <summary>
        /// Returns the next element in a list.
        /// </summary>
        /// <typeparam name="T">Type of the list.</typeparam>
        /// <param name="inputList">List to search.</param>
        /// <param name="currentElement">The element the pointer leads to currently.</param>
        /// <returns>Next element in the list.</returns>
        public static T nextElementInList<T> (List<T> inputList, T currentElement)
        {
            T result = default(T);
            int listLength = inputList.Count;

            //List or current element is empty, abort
            if (listLength == 0 || currentElement == null) 
            {
                return result;
            }

            int currentIndex = inputList.IndexOf(currentElement);

            //Pointer at end of the list, no next element
            if (currentIndex == listLength - 1)
            {
                return currentElement;
            }

            //Regular case, increase pointer by one
            int nextIndex = currentIndex + 1;
            result = inputList[nextIndex];
            return result;
        }

        /// <summary>
        /// Returns the previous element in a list.
        /// </summary>
        /// <typeparam name="T">Type of the list.</typeparam>
        /// <param name="inputList">List to search.</param>
        /// <param name="currentElement">The element the pointer leads to currently.</param>
        /// <returns>Previous element in the list.</returns>
        public static T previousElementInList<T>(List<T> inputList, T currentElement) 
        {
            T result = default(T);
            int listLength = inputList.Count;

            //List or current element is empty, abort
            if (listLength == 0 || currentElement == null)
            {
                return result;
            }

            int currentIndex = inputList.IndexOf(currentElement);

            //Pointer at beginning of the list, no previous element
            if (currentIndex == 0)
            {
                return currentElement;
            }

            //Regular case, decrease pointer by one
            int previousIndex = currentIndex - 1;
            result = inputList[previousIndex];
            return result;

        }
        #endregion

    }

}
