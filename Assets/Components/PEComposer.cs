using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SSA = SSAbstraction;
using UnityEngine.UI;

public class PEComposer : MonoBehaviour
{
    Dictionary<KeyCode, Action> keybindings;
    [SerializeField] PEPartBuilder PEPartBuilder; 
    [SerializeField]RectTransform _buildButton;
    [SerializeField]RectTransform _saveButton;
    [SerializeField]InputField _nameInput;

    GameObject _target;
    GameObject _edgeFrom;
    EdgeSocket _edgeTo;
    SSA.PElement element;
   
    GameObject _PEcanvas => PEPartBuilder.Parent;
    public static PEComposer Main;
    void Start()
    {
        

        foreach(var key in PELibrary.peModelDic.Keys)
        {
            Debug.Log(key);
        }
        Main = this;
        keybindings = new Dictionary<KeyCode, Action>();
        keybindings.Add(KeyCode.Escape, Idle);
        keybindings.Add(KeyCode.P, CreatePEBase);
        keybindings.Add(KeyCode.I, CreateInputNode);
        keybindings.Add(KeyCode.O, CreateOutputNode);
        keybindings.Add(KeyCode.F, CreateFunction);
        keybindings.Add(KeyCode.R, CreateRegister);
    }
    KeyCode GetKey()
    {
        foreach(KeyCode key in keybindings.Keys)
        {
            if (Input.GetKeyDown(key))
                return key;
        }
        return KeyCode.Escape;
    }
    void Update()
    {
        keybindings[GetKey()]();
    }
    
    public void SetTarget(GameObject target)
    {
        if (_target != null)
        {
            this._target.GetComponent<FocusController>().UnFocus();
        }
        target.GetComponent<FocusController>().Focus();
        this._target = target;
    }
    public void SetEdgeFrom(GameObject gObject)
    {
        _edgeFrom = gObject;
    }
    public void SetEdgeTo(GameObject gObject)
    {
        if(_edgeFrom != null)
        {
            _edgeTo = gObject.GetComponent<EdgeSocket>();

            if(_edgeTo.IsLocked)
            {
                Debug.Log("SetEdgeTo: The socket is locked");
                _edgeTo = null;
                return;
            }
            _edgeTo.Lock();
            CreateEdge();
            _edgeTo = null;
        }
        else
        {
            Debug.Log("SetEdgeTo: There is no from part");
        }
    }
    public void BuildPE()
    {
        element = PEAdapter.ToAbstraction(PEPartBuilder.Parent);
        SSA.KSArray arr = new SSA.KSArray(SSA.ArrayForm.box, element, 1, (a, b) => { });
        _PEcanvas.SetActive(false);
        PEPartBuilder.PEBase.gameObject.SetActive(true);
        PEPartBuilder.PEBase.LoadNodes(element.InNodes.Count, element.OutNodes.Count);
        _buildButton.gameObject.SetActive(false);
        _saveButton.gameObject.SetActive(true);
        _nameInput.gameObject.SetActive(true);
    }
    public void SavePE()
    {
        string name = _nameInput.text;
        List<Vector3> inNodesPos = new List<Vector3>();
        List<Vector3> outNodesPos = new List<Vector3>();
        foreach(var node in PEPartBuilder.PEBase.transform.GetChilds((t) => true))
        {
            if(node.GetComponent<PEBaseInNode>() != null)
            {
                inNodesPos.Add(node.transform.localPosition);
            }
            if(node.GetComponent<PEBaseOutNode>() != null)
            {
                outNodesPos.Add(node.transform.localPosition);
            }
        }
        PELibrary.peModelDic.Add(name, new PEModel()
        {
            Name = name,
            PE = element,
            InNodePos = inNodesPos,
            OutNodePos = outNodesPos
        });
        OutputNode.Restart();
        InputNode.Restart();
        Camera.main.GetComponent<SceneTransition>().GoToScene("PE");
    }

    #region BindingFunctions
    void Idle()
    {

    }
    void CreateEdge()
    {
        Debug.Log("CreateEdge");
        PEPartBuilder.CreateEdge(_edgeFrom, _edgeTo.gameObject);
    }
    void CreatePEBase()
    {
        Debug.Log("CreatePEBase");
        PEPartBuilder.CreatePEBase();
    }
    void CreateInputNode()
    {
        Debug.Log("CreateInputNode");
        PEPartBuilder.CreateInputNode();

    }
    void CreateOutputNode()
    {
        Debug.Log("CreateOutputNode");
        PEPartBuilder.CreateOutputNode();
    }
    void CreateFunction()
    {
        Debug.Log("CreateFunction");
        PEPartBuilder.CreateFunction();
    }
    void CreateRegister()
    {
        Debug.Log("CreateRegister");
        PEPartBuilder.CreateRegister();
    }
    #endregion
}
