using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KSAEdge : MonoBehaviour
{
    // Start is called before the first frame update
    public int ToNum => to.Number;
    public int FromNum => from.Number;
    KSAInNode to;
    KSAOutNode from;
    public KSAInNode To => to;
    public KSAOutNode From => from;
    void Start()
    {
        
    }

    public void SetEdgeSocket(GameObject to)
    {
        SetTo(to.GetComponent<KSAInNode>());
    }
    public void SetFrom(KSAOutNode outNode)
    {
        from = outNode;
        
    }
    public void SetTo(KSAInNode inNode)
    {
        to = inNode;
    }
}
