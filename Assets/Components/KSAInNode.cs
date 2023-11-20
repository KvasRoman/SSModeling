using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class KSAInNode : MonoBehaviour
{
    [SerializeField]TextMeshPro text;
    KSAComposer _composer;
    static Color DefaultColor;
    int number;
    int orderNumber;
    bool isLocked;
    public bool ShouldBeInUserInputState = false;
    public bool IsRequiredForUser = false;
    public int OrderNumber => orderNumber;
    public bool IsLocked => isLocked;
    public void Lock()
    {
        isLocked = true;
    }
    public void UnLock()
    {
        isLocked = false;
    }
    public void SetInputOrderNumber(int number)
    {
        orderNumber = number;
    }
    public int Number => number;
    void Start()
    {
        DefaultColor = new Color(211, 255, 0);
        if (text == null)
            text = transform.GetChilds((t) => t.GetComponent<TextMeshPro>() != null)[0].transform.GetComponent<TextMeshPro>();
        _composer = Camera.main.GetComponent<KSAComposer>();
    }
    private void OnMouseDown()
    {
        if (ShouldBeInUserInputState)
        {
            IsRequiredForUser = !IsRequiredForUser;
            GetComponent<SpriteRenderer>().color = IsRequiredForUser ? new Color(0, 255, 0) : DefaultColor;
        }
        if (isLocked) return;
        else
        _composer.SetEdgeTo(this);
    }
    public void SetNumber(int num)
    {
        number = num;
        text.text = number.ToString();
    }
}
