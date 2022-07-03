using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EdgeSocket : MonoBehaviour
{
    bool isLocked;
    public bool IsLocked => isLocked;
    public void Lock()
    {
        isLocked = true;
    }
    public void UnLock()
    {
        isLocked = false;
    }
    private void OnMouseDown()
    {
        PEComposer.Main.SetEdgeTo(gameObject);
    }
}
