using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class FocusController : MonoBehaviour, IFocusable
{
    static List<IFocusable> focusables = new List<IFocusable>();
    BoxCollider2D collinder;
    void Start()
    {
        foreach (IFocusable focusable in focusables)
        {
            focusable.UnFocus();
        }
            
        focusables.Add(this);
        Camera.main.GetComponent<PEComposer>().SetTarget(this.gameObject);
        collinder = GetComponent<BoxCollider2D>();
    }
    private void OnMouseDown()
    {
        Camera.main.GetComponent<PEComposer>().SetTarget(gameObject);
    }

    public void Focus()
    {
        transform.GetChilds(null).ForEach((t) =>
        {
            var tsr = t.GetComponent<SpriteRenderer>();
            if (tsr != null) tsr.enabled = true;
            var tbc = t.GetComponent<BoxCollider2D>();
            if (tbc != null) tbc.enabled = true;
        });
    }
    
    private void OnDestroy()
    {
        var edges = transform.GetChilds((t) => {
            return t.GetComponent<Edge>() != null;
        });
        edges.ForEach((edge) =>
        {
            edge.GetComponent<Edge>().UnLockSocket();
        });
        focusables.Remove(this);
    }
    public void UnFocus()
    {
        transform.GetChilds((t) => t.GetComponent<EdgeSocket>() == null && t.GetComponent<Edge>() == null).ForEach((t) =>
        {
            var tsr = t.GetComponent<SpriteRenderer>();
            if (tsr != null) tsr.enabled = false;
            var tbc = t.GetComponent<BoxCollider2D>();
            if(tbc != null) tbc.enabled = false;
        });
    }
}
