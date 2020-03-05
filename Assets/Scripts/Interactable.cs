using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header ("Attributes")]
    [SerializeField]
    private int m_ID;

    public int GetID()
    {
        return m_ID;
    }
    public void SetID(int n)
    {
        m_ID = n;
    }
}
