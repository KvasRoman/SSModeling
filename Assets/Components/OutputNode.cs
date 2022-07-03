using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OutputNode : MonoBehaviour
{
    TextMeshPro text;

    static int number = 0;
    void Start()
    {
        text = transform.GetChilds((t) => t.GetComponent<TextMeshPro>() != null)[0].transform.GetComponent<TextMeshPro>();
        number++;
        SetNumber(number);
    }
    void SetNumber(int number)
    {
        text.text = number.ToString();
    }
    void Update()
    {

    }
    public static void Restart()
    {
        number = 0;
    }
}
