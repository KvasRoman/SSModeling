using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INamable
{
    string Name { get; }
    void SetName(string name); 
}
