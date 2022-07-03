using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class KSAOutNode : MonoBehaviour
{
    [SerializeField]TextMeshPro text;
    KSAComposer _composer;
    int number;
    public int Number => number;
    void Start()
    {
        if(text == null)
            text = transform.GetChilds((t)=> t.GetComponent<TextMeshPro>() != null)[0].transform.GetComponent<TextMeshPro>();
        _composer = Camera.main.GetComponent<KSAComposer>();
    }
    private void OnMouseDown()
    {
        _composer.SetEdgeFrom(this);
    }
    public void SetNumber(int num)
    {
        number = num;
        UpdateText();
    }
    void UpdateText()
    {
        text.text = number.ToString();
    }
}
