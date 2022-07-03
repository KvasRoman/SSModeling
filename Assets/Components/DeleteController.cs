using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DeleteController : MonoBehaviour
{
    bool _wasDown = false;
    private void OnMouseDown()
    {
        _wasDown = true;
    }
    void OnMouseExit()
    {
        _wasDown = false;
    }
    private void OnMouseUp()
    {
        if (_wasDown)
            Object.Destroy(transform.parent.gameObject);

    }
}
