using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QSAphaseswitcher : MonoBehaviour
{
    [SerializeField]
    GameObject canvas;


    Action<string> subscribtions;
    void Start()
    {
    }
    public void Subscribe(Action<string> callback)
    {
        subscribtions += callback;
    }
    public void SwitchToPhase(int phase)
    {
        var children = canvas.transform.GetChilds(child => child.name.StartsWith("Phase"));
        
        var phaseFullName = "Phase " + phase.ToString();

        subscribtions(phaseFullName);

        foreach (var child in children)
        {

            child.transform.localScale = Vector3.zero;
            if (child.name == phaseFullName)
                child.transform.localScale = Vector3.one;
        }
    }
}
