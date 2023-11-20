using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class KSAOutNode : MonoBehaviour
{
    [SerializeField]TextMeshPro text;
    KSAComposer _composer;
    static Color DefaultColor;
    int orderNumber;
    public int OrderNumber => orderNumber;
    int number;
    public int Number => number;
    bool isLocked;
    public bool IsLocked => isLocked;
    public bool ShouldBeInUserInputState = false;
    public bool IsRequiredForUser = false;
    public void Lock()
    {
        isLocked = true;
    }
    public void Unlock()
    {
        isLocked = false;
    }
    public void SetOutputOrderNumber(int number)
    {
        orderNumber = number;
    }

    void Start()
    {
        DefaultColor = new Color(1f, 0.6196079f, 0);
        if (text == null)
            text = transform.GetChilds((t)=> t.GetComponent<TextMeshPro>() != null)[0].transform.GetComponent<TextMeshPro>();
        _composer = Camera.main.GetComponent<KSAComposer>();
    }
    private void OnMouseDown()
    {
        if(ShouldBeInUserInputState)
        {
            IsRequiredForUser = !IsRequiredForUser;
            GetComponent<SpriteRenderer>().color = IsRequiredForUser ? new Color(1f, 0.364f, 0) : DefaultColor;
        }
        if(isLocked) return;
        else
        {
            _composer.SetEdgeFrom(this);
        }
        
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
