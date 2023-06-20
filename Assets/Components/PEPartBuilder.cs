using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PEPartBuilder : MonoBehaviour 
{
    [SerializeField] GameObject parent;
    [SerializeField] PEBase _PEBase;
    public PEBase PEBase => _PEBase;
    public GameObject Parent => parent;
    Vector2 CursorPos => Camera.main.ScreenToWorldPoint(Input.mousePosition);

    public Dictionary<int,Transform> Parts = new Dictionary<int,Transform>();

    void Start()
    {
     
    }
    public void BindTo(GameObject parent)
    {
        this.parent = parent;
    }
    GameObject LoadPrefab(string prefabName)
    {
        return Resources.Load<GameObject>("Entities/PE/" + prefabName);
    }
    GameObject LoadPEBasePrefab(string prefabName)
    {
        return Resources.Load<GameObject>("Entities/PEBase/" + prefabName);
    }
    void ConnectFromTo(GameObject gObj, Vector2 pos1, Vector2 pos2)
    {
        Vector3 lineCenter = pos1 + (pos2 - pos1)/2;
        lineCenter.z = gObj.transform.position.z;
        Vector2 diff = pos2 - pos1;
        Vector3 rotation = Vector3.zero;
        gObj.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(diff.y,diff.x) * 180 / Mathf.PI);
        Vector3 scale = gObj.transform.localScale;
        scale.x = (pos2 - pos1).magnitude;
        Debug.Log(scale.x);
        Debug.Log(pos1.ToString() + pos2.ToString() + diff.ToString());
        gObj.transform.localScale = scale;
        gObj.transform.position = lineCenter;
    }
    #region Parts of PE
    public GameObject CreatePart(string partName)
    {
        var obj = Object.Instantiate(LoadPrefab(partName), parent.transform);
        Parts.Add(obj.GetInstanceID(), obj.transform);
        return obj;
    }
    public GameObject CreatePart(string partName, GameObject parent)
    {
        var obj = Object.Instantiate(LoadPrefab(partName), parent.transform);
        Parts.Add(obj.GetInstanceID(), obj.transform);
        return obj;
    }
    public void CreatePEBase()
    {
        CreatePart("PEBase").SetPos(CursorPos);
        
    }
    public void CreateInputNode()
    {
        CreatePart("InputNode").SetPos(CursorPos);

    }
    public void CreateOutputNode()
    {
        CreatePart("OutputNode").SetPos(CursorPos);
    }
    public void CreateFunction()
    {
        CreatePart("Function").SetPos(CursorPos);
    }
    public void CreateRegister()
    {
        CreatePart("Register").SetPos(CursorPos);
    }
    public void CreateEdge(GameObject from, GameObject to)
    {
        var edge = CreatePart("Edge", from);
        from.transform.parent.Find("DragController").GetComponent<DragController>().OnDragSubscribe(() =>
        {
            ConnectFromTo(edge, from.transform.position, to.transform.position);
        });
        to.transform.parent.Find("DragController").GetComponent<DragController>().OnDragSubscribe(() =>
        {
            ConnectFromTo(edge, from.transform.position, to.transform.position);
        });
        edge.GetComponent<Edge>().SetEdgeSocket(to);
        ConnectFromTo(edge, from.transform.position, to.transform.position);
    }
    #endregion
    public void CreateInNodePos(Vector3 localPos, Transform parent, int number)
    {
        var obj = Object.Instantiate(LoadPEBasePrefab("InNode"), parent.transform);
        obj.transform.GetChilds((t) => t.GetComponent<TextMeshPro>() != null)[0].GetComponent<TextMeshPro>().text = number.ToString();
        obj.GetComponent<PEBaseInNode>().SetNumber(number);
        obj.transform.localPosition = localPos;
    }
    public void CreateOutNodePos(Vector3 localPos, Transform parent, int number)
    {
        var obj = Object.Instantiate(LoadPEBasePrefab("OutNode"), parent.transform);
        obj.transform.GetChilds((t) => t.GetComponent<TextMeshPro>() != null)[0].GetComponent<TextMeshPro>().text = number.ToString();
        obj.GetComponent<PEBaseOutNode>().SetNumber(number);
        obj.transform.localPosition = localPos;
    }

}
