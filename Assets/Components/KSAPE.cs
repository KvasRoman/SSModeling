using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SSA = SSAbstraction;
[RequireComponent(typeof(BoxCollider2D))]
public class KSAPE : MonoBehaviour, IFocusable
{
    SSA.PElement element;
    public SSA.PElement PE => element;
    KSAComposer _composer;
    List<KSAEdge> _edges;
    event Action onDrag;
    public void Focus()
    {
        transform.GetChilds((t) => true).ForEach((e) => e.SetActive(true));
    }
    public void UnFocus()
    {
        var children = transform.GetChilds((t) => t.GetComponent<KSAInNode>() == null && t.GetComponent<KSAOutNode>() == null && t.GetComponent<KSAEdge>() == null);
        foreach(var child in children)
        {
            child.SetActive(false);
        }
    }
    private void OnDestroy()
    {
        _composer.RemoveFocusable(this);
        var edges = transform.GetChilds((edge) => edge.GetComponent<KSAEdge>() != null);
        foreach(var edge in edges)
        {
            edge.GetComponent<KSAEdge>().To.UnLock();
        }
    }
    private void OnMouseDown()
    {
        _composer.SetTarget(this);
        
    }
    private void OnDrag(GameObject gObj, Vector3 pos1, Vector3 pos2)
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
    public void AddOnDragEvent(Action action)
    {
        onDrag += action;
    }
    void Start()
    {
        _composer = Camera.main.GetComponent<KSAComposer>();
        _composer.AddToFocusables(this);
        _composer.SetTarget(this);
        transform.GetChilds(e => e.GetComponent<DragController>() != null)[0].GetComponent<DragController>().OnDragSubscribe(() =>
        {
            onDrag?.Invoke();
            var edges = transform.GetChilds(e => e.GetComponent<KSAEdge>() != null);
            foreach (var gObjeEdge in edges)
            {
                var edge = gObjeEdge.GetComponent<KSAEdge>();
                OnDrag(edge.gameObject, edge.From.transform.position, edge.To.transform.position);
            }
                
        });
    }
    public void SetPE(SSA.PElement element)
    {
        this.element = element.Clone();
    }
}
