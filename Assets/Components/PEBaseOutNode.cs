using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEBaseOutNode : MonoBehaviour
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
