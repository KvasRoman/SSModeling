using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SSAbstraction;
public class KSABuilder : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject _KSACanvas;
    public GameObject KSACanvas => _KSACanvas;
    Vector2 CursorPos =>  Camera.main.ScreenToWorldPoint(Input.mousePosition);
    void Start()
    {
        
    }
    GameObject LoadPrefab(string prefabName)
    {
        return Resources.Load<GameObject>("Entities/KSA/" + prefabName);
    }

    public void CreatePE(PEModel model)
    {
        var obj = Object.Instantiate(LoadPrefab("PE"), KSACanvas.transform);
        obj.GetComponent<KSAPE>().SetPE(model.PE);
        obj.transform.position = CursorPos;
        int i = 1;
        foreach(var inN in model.InNodePos)
        {
            CreateInNode(obj, i++, inN);
        }
        i = 1;
        foreach (var outN in model.OutNodePos)
        {
            CreateOutNode(obj, i++, outN);
        }
    }
    public void CreateInNode(GameObject PE, int number, Vector3 localPos)
    {
        var obj = Object.Instantiate(LoadPrefab("InNode"), PE.transform);
        obj.transform.localPosition = localPos;
        obj.GetComponent<KSAInNode>().SetNumber(number);
    }
    public void CreateOutNode(GameObject PE, int number, Vector3 localPos)
    {
        var obj = Object.Instantiate(LoadPrefab("OutNode"), PE.transform);
        obj.transform.localPosition = localPos;
        obj.GetComponent<KSAOutNode>().SetNumber(number);
    }
    void ConnectFromTo(GameObject gObj, Vector2 pos1, Vector2 pos2)
    {
        Vector3 lineCenter = pos1 + (pos2 - pos1) / 2;
        lineCenter.z = gObj.transform.position.z;
        Vector2 diff = pos2 - pos1;
        Vector3 rotation = Vector3.zero;
        gObj.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(diff.y, diff.x) * 180 / Mathf.PI);
        Vector3 scale = gObj.transform.localScale;
        scale.x = (pos2 - pos1).magnitude;
        Debug.Log(scale.x);
        Debug.Log(pos1.ToString() + pos2.ToString() + diff.ToString());
        gObj.transform.localScale = scale;
        gObj.transform.position = lineCenter;
    }
    public void CreateEdge(GameObject from, GameObject to)
    {
        if (to.GetComponent<KSAInNode>().IsLocked)
        {
            return;
        }
        var edge = Object.Instantiate(LoadPrefab("Edge"), from.transform.parent);
        edge.GetComponent<KSAEdge>().SetEdgeSocket(to);
        to.GetComponent<KSAInNode>().Lock();
        edge.GetComponent<KSAEdge>().SetFrom(from.GetComponent<KSAOutNode>());
        ConnectFromTo(edge, from.transform.position, to.transform.position);
    }
    public KSArray Build()
    {
        KSArray kSArray = null;
        var pes = _KSACanvas.transform.GetChilds((t) => t.GetComponent<KSAPE>() != null);
        var array = new List<PElement>();
        foreach(var pe in pes)
        {
            array.Add(pe.GetComponent<KSAPE>().PE);
        }
        foreach (var pe in pes)
        {
            var edges = pe.transform.GetChilds((t) => t.GetComponent<KSAEdge>() != null);
            var from = pe.GetComponent<KSAPE>().PE;
            foreach (var edge in edges)
            {
                var to = edge.GetComponent<KSAEdge>().To.transform.parent.GetComponent<KSAPE>().PE;
                var aEdge = edge.GetComponent<KSAEdge>();
                from.ConnectTo(to, aEdge.ToNum - 1, aEdge.FromNum - 1);
            }
        }
        try
        {
            kSArray = new KSArray(array);
        }
        catch(System.Exception ex)
        {
            Debug.Log(ex);
            return null;
        }
        return kSArray;
    }
}
