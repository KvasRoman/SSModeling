using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SSA = SSAbstraction;
public class KSAComposer : MonoBehaviour
{
    static List<IFocusable> focusables = new List<IFocusable>();
    [SerializeField] KSABuilder builder = null;
    [SerializeField] Dropdown PEList;
    [SerializeField] TMP_InputField inputText;
    [SerializeField] Text outputText;
    Dictionary<KeyCode, Action> _keybindings;
    SSA.KSArray _activeBuild;
    KSAInNode _to;
    KSAOutNode _from;
    IFocusable _target;
    PEModel _selectedModel;
    GameObject KSACanvas;
    KeyCode getKey()
    {
        foreach(var key in _keybindings.Keys)
        {
            if (Input.GetKeyUp(key))
                return key;
        }
        return KeyCode.None;
    }
    
    void Start()
    {
        KSACanvas = GetComponent<KSABuilder>().KSACanvas;
        Camera.main.GetComponent<QSAphaseswitcher>().Subscribe(SwitchToPhaseTwo);

        if (PELibrary.peModelDic != null)
        {
            var optionList = new List<Dropdown.OptionData>();
            foreach (var key in PELibrary.peModelDic.Keys)
            {
                optionList.Add(new Dropdown.OptionData(PELibrary.peModelDic[key].Name));
            }
            PEList.AddOptions(optionList);
            PEList.onValueChanged.AddListener((i) => { SelectModel(i); });
            if(PELibrary.peModelDic.Keys.Count != 0)
                _selectedModel = PELibrary.peModelDic[PEList.options[0].text];
        }
        _keybindings = new Dictionary<KeyCode, Action>();
        _keybindings.Add(KeyCode.None, () => { });
        _keybindings.Add(KeyCode.P, CreatePE);
    }
    void Update()
    {
        _keybindings[getKey()]();
    }
    public void CreateEdge()
    {
        builder.CreateEdge(_from.gameObject, _to.gameObject);
    }
    public void SetEdgeFrom(KSAOutNode outNode)
    {
        _from = outNode;
    }
    public void SetEdgeTo(KSAInNode inNode)
    {
        if (_from == null) return;
        _to = inNode;
        CreateEdge();
        _from = null;
        _to = null;
    }
    public void RemoveFocusable(IFocusable focusable)
    {
        focusables.Remove(focusable);
    }
    public void SelectModel(int num)
    {
        _selectedModel = PELibrary.peModelDic[PEList.options[num].text];
    }
    public void SetTarget(IFocusable focusable)
    {
        foreach(var foc in focusables)
        {
            foc.UnFocus();
        }
        focusable.Focus();
    }
    public void AddToFocusables(IFocusable focusable)
    {
        focusables.Add(focusable);
    }
    public void BuildKSS()
    {
        _activeBuild = builder.Build();
        if(_activeBuild != null )
        {
            //Phase 2 configure input order
            Camera.main.GetComponent<QSAphaseswitcher>().SwitchToPhase(2);
        }
        else
        {
            Debug.Log("Error occured during QSS build");
        }
    }
    public void LoadData()
    {
        LoadData(inputText.text);
    }
    void LoadData(string data)
    {
        var datarows = data.Split('\n');
        List<List<double>> formedData = new List<List<double>>();
        for (int i = 0; i < datarows[0].Split(' ').Length; i++)
            formedData.Add(new List<double>());
        
        foreach (var datarow in datarows)
        {
            var doubles = datarow.Split(' ');
            for (int i = 0; i < doubles.Length; i++)
                formedData[i].Add(double.Parse(doubles[i]));
            Debug.Log(datarow);
        }
        _activeBuild.AddInputValues(formedData);    
    }
    public void StartTact()
    {
        _activeBuild.TactStart();
        outputText.text = _activeBuild.GetCollectorsValues();
    }

    public void SwitchToPhaseTwo(string phase)
    {
        if (phase != "Phase 2")
        {
            return;
        }
        var PEs = KSACanvas.transform.GetChilds(a => true);
        List<KSAEdge> edges = new List<KSAEdge>();
        foreach (var pe in PEs)
        {
            var tempEdgeList = pe.transform.GetChilds(e => e.GetComponent<KSAEdge>() != null);
            foreach (var edge in tempEdgeList)
            {
                edges.Add(edge.GetComponent<KSAEdge>());
            }

            //hide all OutNodes
            pe.transform.GetChilds(c => c.GetComponent<KSAOutNode>() != null)[0].transform.localScale = Vector3.zero;
        }
        foreach (var edge in edges)
        {
            edge.To.transform.localScale = Vector3.zero;
        }
        
    }
    #region keybinding functions
    void CreatePE()
    {
        builder.CreatePE(_selectedModel);
    }


    #endregion

}
