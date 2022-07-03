using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEBaseInNode : MonoBehaviour
{
    int _number;
    int Number => _number;

    void Start()
    {
        
    }
    public void SetNumber(int number)
    {
        _number = number;
    }
}
