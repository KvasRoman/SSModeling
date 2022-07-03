using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class AddEdgeController : MonoBehaviour
{
    private void OnMouseDown()
    {
        PEComposer.Main.SetEdgeFrom(gameObject);
    }

}
