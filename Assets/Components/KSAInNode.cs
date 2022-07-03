using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class KSAInNode : MonoBehaviour
{
    [SerializeField]TextMeshPro text;
    KSAComposer _composer;
    int number;
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
    public int Number => number;
    void Start()
    {
        if (text == null)
            text = transform.GetChilds((t) => t.GetComponent<TextMeshPro>() != null)[0].transform.GetComponent<TextMeshPro>();
        _composer = Camera.main.GetComponent<KSAComposer>();
    }
    private void OnMouseDown()
    {
        _composer.SetEdgeTo(this);
    }
    public void SetNumber(int num)
    {
        number = num;
        text.text = number.ToString();
    }
}
