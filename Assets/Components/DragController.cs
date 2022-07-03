using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DragController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool Active = true;
    Transform _parent;
    Collider2D _colider;
    private void Start()
    {
        _colider = GetComponent<Collider2D>();
        _parent = transform.parent;
    }
    Vector3 CursorPos => Camera.main.ScreenToWorldPoint(Input.mousePosition);
    private void OnMouseDrag()
    {
        Vector3 pos = CursorPos;
        pos.z = _parent.position.z;
        _parent.position = pos;
    }
}
