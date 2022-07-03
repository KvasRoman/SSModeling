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
    }
    private void OnMouseDown()
    {
        _composer.SetTarget(this);
        
    }
    void Start()
    {
        _composer = Camera.main.GetComponent<KSAComposer>();
        _composer.AddToFocusables(this);
        _composer.SetTarget(this);
    }
    public void SetPE(SSA.PElement element)
    {
        this.element = element.Clone();
    }
}
