using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
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
    [SerializeField] TMP_InputField defaultValue;
    [SerializeField] TMP_InputField dataPenalty;
    Dictionary<KeyCode, Action> _keybindings;
    SSA.KSArray _activeBuild;
    KSAInNode _to;
    KSAOutNode _from;
    IFocusable _target;
    PEModel _selectedModel;
    GameObject KSACanvas;
    List<(TMP_Text text, int order)> outputTextList = new List<(TMP_Text text, int order)>();
    
    KeyCode getKey()
    {
        foreach (var key in _keybindings.Keys)
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
            if (PELibrary.peModelDic.Keys.Count != 0)
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
        foreach (var foc in focusables)
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
        if (_activeBuild != null)
        {
            //Phase 2 configure input order
            Camera.main.GetComponent<QSAphaseswitcher>().SwitchToPhase(2);
        }
        else
        {
            Debug.Log("Error occured during QSS build");
        }
    }
    public void SaveRequiredInputsOutputs()
    {

        string output = "";
        string output2 = "";
        var resInput = builder.GetRequiredInputNumbers();
        var resOutput = builder.GetRequiredOutputNumbers();
        foreach (var i in resInput)
        {
            output += " ";
            output += i.ToString();
        }
        foreach (var i in resOutput)
        {
            output2 += " ";
            output2 += i.ToString();
        }
        Debug.Log(output);
        Debug.Log(output2);
        builder.AddInputFields();
        
        var outputs = builder.KSACanvas.transform.GetChilds(c => c.GetComponent<KSAPE>() != null);
        foreach (var i in outputs)
        {
            var peOutputs = i.transform.GetChilds(c => c.GetComponent<KSAOutNode>() != null);
            foreach(var j in peOutputs)
            {
                var pureOutput = j.GetComponent<KSAOutNode>();
                if (pureOutput.IsRequiredForUser)
                {
                    var outputField = builder.AddOutputField(j).transform.GetChilds(c => c.GetComponent<TMP_Text>() != null)[0].GetComponent<TMP_Text>();
                    outputTextList.Add((outputField, pureOutput.OrderNumber));
                }
                
                
            }
        }
        Camera.main.GetComponent<QSAphaseswitcher>().SwitchToPhase(3);
    }
    public void SaveInputData()
    {
        // !!add qsa element select!!

        //
        List<int> order;
        var values = builder.GetInputValues(out order);
        for (var i = 0; i < order.Count; i++)
        {
            order[i] -= 1;
        }
        /*
        string text1 = "";
        string text2 = "";
        foreach(var value in values)
        {
            foreach(var i in value)
            {

                text1 += i.ToString();
                text1 += " ";
            }
        }
        foreach(var value in order)
        {
            text2 += value.ToString();
            text2 += " ";
        }
        Debug.Log(text1);
        Debug.Log(text2);
        */

        int maxInputHeight = 0;
        int penalty; 
        if(!int.TryParse(dataPenalty.text,out penalty))
        {
           penalty = 0;
        }
        double defaultV;
        if(!double.TryParse(defaultValue.text,out defaultV))
        {
            defaultV = 0;
        }
        foreach (var i in values)
        {
            if(maxInputHeight < i.Count)
                maxInputHeight = i.Count;
        }
        int width = _activeBuild.InputWidth;
        int heigh = maxInputHeight + penalty;
        List<List<double>> input = new List<List<double>>();
        for(var i = 0;i < width; i++)
        {
            
            if (order.Contains(i))
            {
                List<double> doubles = new List<double>();
                for(var k = 0;k < penalty; k++)
                {
                    doubles.Add(defaultV);
                }
                if (values[i].Count < maxInputHeight)
                {
                    for(var k = 0; k < maxInputHeight - values[i].Count; k++)
                    {
                        doubles.Add(0);
                    }
                }
                doubles = doubles.Concat(values[i]).ToList();
                input.Add(doubles);
            }
            else
            {
                input.Add(new List<double>());
                for (var k = 0; k < heigh; k++)
                {
                    input[i].Add(defaultV);
                }
            }
            
        }
        
        _activeBuild.AddInputValues(input);

        Camera.main.GetComponent<QSAphaseswitcher>().SwitchToPhase(4);
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
        var res = _activeBuild.GetCollectorsValues();
        Debug.Log(res);
        return;
        /*
        foreach(var output in outputTextList)
        {
            string text = "";
            foreach (var item in res[output.order - 1])
            {
                text += item.ToString();
                text += " ";
            }
            output.text.text = text;
        }
        */
        //outputText.text = _activeBuild.GetCollectorsValues();
    }

    public void SwitchToPhaseTwo(string phase)
    {
        
        if (phase != "Phase 2")
        {
            return;
        }
        var PEs = KSACanvas.transform.GetChilds(a => true);
        List<KSAEdge> edges = new List<KSAEdge>();
        int orderOutputNumber = 1;
        int orderInputNumber = 1;
        foreach (var pe in PEs)
        {
            var tempEdgeList = pe.transform.GetChilds(e => e.GetComponent<KSAEdge>() != null);

            foreach (var edge in tempEdgeList)
            {
                var pureEdge = edge.GetComponent<KSAEdge>();
                edges.Add(pureEdge);
                pureEdge.From.Lock();
                pureEdge.To.Lock();
            }
            
            var tempInNodeList = pe.transform.GetChilds(inNode => inNode.GetComponent<KSAInNode>() != null);
            foreach (var inNode in tempInNodeList)
            {
                var pureInNode = inNode.GetComponent<KSAInNode>();
                if (!pureInNode.IsLocked)
                {
                    pureInNode.SetInputOrderNumber(orderInputNumber++);
                    pureInNode.Lock();
                    pureInNode.ShouldBeInUserInputState = true;
                }
                    
            }
            var tempOutNodeList = pe.transform.GetChilds(inNode => inNode.GetComponent<KSAOutNode>() != null);
            foreach (var inNode in tempOutNodeList)
            {
                var pureOutNode = inNode.GetComponent<KSAOutNode>();
                if (!pureOutNode.IsLocked)
                {
                    pureOutNode.SetOutputOrderNumber(orderOutputNumber++);
                    pureOutNode.Lock();
                    pureOutNode.ShouldBeInUserInputState = true;
                }
                    
            }
            //hide all OutNodes
            //pe.transform.GetChilds(c => c.GetComponent<KSAOutNode>() != null)[0].transform.localScale = Vector3.zero;
        }
        foreach (var edge in edges)
        {

            edge.To.transform.localScale = Vector3.zero;
            edge.From.transform.localScale = Vector3.zero;
        }

    }
    #region keybinding functions
    void CreatePE()
    {
        builder.CreatePE(_selectedModel);
    }


    #endregion

}
