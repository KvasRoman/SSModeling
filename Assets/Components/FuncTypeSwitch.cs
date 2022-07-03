using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FuncTypeSwitch : MonoBehaviour
{
    // Start is called before the first frame update
    TextMeshPro text;
    void Start()
    {
        text = transform.GetChilds((c) => c.GetComponent<TextMeshPro>() != null)[0].GetComponent<TextMeshPro>();
    }
    public void UpdateFuncType(bool left)
    {
        text.text = transform.parent.GetComponent<Function>().UpdateFunctionType(left);
    }
}
