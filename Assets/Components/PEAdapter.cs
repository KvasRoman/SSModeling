using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SSA = SSAbstraction;
public class PEAdapter
{
    static Dictionary<int, string> fPairs = new Dictionary<int, string>();
    static Dictionary<int, string> rPairs = new Dictionary<int, string>();
    static Dictionary<int, string> outNPairs = new Dictionary<int, string>();
    static Dictionary<int, string> inNPairs = new Dictionary<int, string>();
    public static SSA.PElement ToAbstraction(GameObject PE)
    {
        
        SSA.PElement res = null;
        List<SSA.FunctionA2> funcs = getFuncs(PE,out fPairs);
        List<SSA.Register> registers = getRegs(PE, out rPairs);
        List<SSA.OutputNode> outNodes = getOutNodes(PE, out outNPairs);
        List<SSA.InputNode> inputNodes = getInNodes(PE, out inNPairs);
        List<SSA.Edge> edges = new List<SSA.Edge>();
        List<Edge> uEdges = new List<Edge>();
        
        var elements = PE.transform.GetChilds((t) => true);
        
        foreach(var element in elements)
        {
            var unityEdges = element.transform.GetChilds((t) => t.GetComponent<Edge>() != null);
            SSA.IRepeater from =
                element.GetComponent<InputNode>() != null ? (SSA.IRepeater)inputNodes.Find((i) => i.Id == inNPairs[element.GetComponent<InputNode>().GetInstanceID()]) :
                element.GetComponent<Function>() != null ? (SSA.IRepeater)funcs.Find((f) => f.Id == fPairs[element.GetComponent<Function>().GetInstanceID()]) :
                element.GetComponent<Register>() != null ? (SSA.IRepeater)registers.Find((r) => r.Id == rPairs[element.GetComponent<Register>().GetInstanceID()]) : null;
            if (from == null) continue;
            foreach (var unityEdge in unityEdges)
            {
                var unityTo = unityEdge.GetComponent<Edge>().GetSocket().transform.parent;
                SSA.IDataReciever to =
                    unityTo.GetComponent<Function>() != null ? (SSA.IDataReciever)funcs.Find((f) => f.Id == fPairs[unityTo.GetComponent<Function>().GetInstanceID()]) :
                    unityTo.GetComponent<Register>() != null ? (SSA.IDataReciever)registers.Find((r) => r.Id == rPairs[unityTo.GetComponent<Register>().GetInstanceID()]) :
                    unityTo.GetComponent<OutputNode>() != null ? (SSA.IDataReciever)outNodes.Find((outN) => outN.Id == outNPairs[unityTo.GetComponent<OutputNode>().GetInstanceID()]) : null;
                if(to == null) Debug.Log("Edge goes nowhere");
                from.AddEdge(new SSA.Edge(to));
            }
            
        }

        
        res = new SSA.PElement(inputNodes, outNodes, registers, funcs);

        
        return res;
    }
    static List<SSA.FunctionA2> getFuncs(GameObject PE, out Dictionary<int, string> keyValuePairs) {
        Dictionary<int, string> AbstToEntity = new Dictionary<int, string>();
        List<SSA.FunctionA2> funcs = new List<SSA.FunctionA2>();
        List<GameObject> unityFuncs = PE.transform.GetChilds((pe) => pe.GetComponent<Function>() != null);
        foreach (var func in unityFuncs)
        {
            var tempF = func.GetComponent<Function>();
            var tempAbstF = new SSA.FunctionA2(GetFunc(tempF.fType));
            funcs.Add(
                tempAbstF
                );
            AbstToEntity.Add(tempF.GetInstanceID(), tempAbstF.Id);
        }
        keyValuePairs = AbstToEntity;
        return funcs;
    }
    static List<SSA.Register> getRegs(GameObject PE, out Dictionary<int, string> keyValuePairs) {
        Dictionary<int, string> AbstToEntity = new Dictionary<int, string>();
        List<SSAbstraction.Register> regs = new List<SSAbstraction.Register>();
        List<GameObject> unityRegs = PE.transform.GetChilds((pe) => pe.GetComponent<Register>() != null);
        foreach (var reg in unityRegs)
        {
            var tempR = reg.GetComponent<Register>();
            var tempAbstR = new SSAbstraction.Register();
            regs.Add(
                tempAbstR
                );
            AbstToEntity.Add(tempR.GetInstanceID(), tempAbstR.Id);
        }
        keyValuePairs = AbstToEntity;
        return regs;
    }
    static List<SSA.InputNode> getInNodes(GameObject PE, out Dictionary<int, string> keyValuePairs) {
        Dictionary<int, string> AbstToEntity = new Dictionary<int, string>();
        List<SSA.InputNode> inNodes = new List<SSA.InputNode>();
        List<GameObject> unityInNodes = PE.transform.GetChilds((pe) => pe.GetComponent<InputNode>() != null);
        foreach (var reg in unityInNodes)
        {
            var tempInNode = reg.GetComponent<InputNode>();
            var tempAbstInNode = new SSA.InputNode();
            inNodes.Add(
                tempAbstInNode
                );
            AbstToEntity.Add(tempInNode.GetInstanceID(), tempAbstInNode.Id);
        }
        keyValuePairs = AbstToEntity;
        return inNodes;
    }
    static List<SSA.OutputNode> getOutNodes(GameObject PE, out Dictionary<int, string> keyValuePairs) {
        Dictionary<int, string> AbstToEntity = new Dictionary<int, string>();
        List<SSA.OutputNode> outNodes = new List<SSA.OutputNode>();
        List<GameObject> unityOutNodes = PE.transform.GetChilds((pe) => pe.GetComponent<OutputNode>() != null);
        foreach (var reg in unityOutNodes)
        {
            var tempOutNode = reg.GetComponent<OutputNode>();
            var tempAbstOutNode = new SSA.OutputNode();
            outNodes.Add(
                tempAbstOutNode
                );
            AbstToEntity.Add(tempOutNode.GetInstanceID(), tempAbstOutNode.Id);
        }
        keyValuePairs = AbstToEntity;
        return outNodes;
    }

    static Func<double, double, double> GetFunc(string name)
    {
        Debug.Log("Name: " + name);
        switch (name)
        {
            case "+": return (a,b) => a + b; 
            case "*": return (a, b) => a * b;
            case "-": return (a, b) => a - b;
            case "/": return (a, b) => a / b;
            default: throw new Exception();
        }
    }
}
