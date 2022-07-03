using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SSAbstraction;
public class PEAbstractionSrorage
{
   static Dictionary<string,PElement> elements = new Dictionary<string, PElement>();
   static void AddPElement(string name, PElement element)
    {
        if(!elements.ContainsKey(name))
        elements.Add(name, element);
        else
        {
            Debug.Log(name + " is already exists");
        }
    }
}
