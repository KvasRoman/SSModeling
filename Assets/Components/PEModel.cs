using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SSA = SSAbstraction;
public class PEModel
{
    public SSA.PElement PE { get; set; }
    public string Name { get; set; }
    public List<Vector3> InNodePos { get; set; }
    public List<Vector3> OutNodePos { get; set; }
}
