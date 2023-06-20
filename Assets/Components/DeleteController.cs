using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DeleteController : MonoBehaviour
{
    bool _wasDown = false;
    event Action subscribersHandler;
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
        {
            subscribersHandler?.Invoke();
            UnityEngine.Object.Destroy(transform.parent.gameObject);
        }
            

    }
    public void OnDeleteSubscribe(Action action)
    {
        subscribersHandler += action;
    }
}
