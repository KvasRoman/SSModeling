using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Function : MonoBehaviour
{
    static List<string> functions = new List<string>()
    {
        "+",
        "-",
        "*",
        "/"
    };
    string type;
    public string fType => type;
    void Start()
    {
        type = functions[0];
    }
    public string UpdateFunctionType(bool left)
    {
        string res;
        int index = functions.FindIndex((s) => s == type);
        if (left) res = functions[
            index == functions.Count - 1? 0 : index + 1
            ];
        else res = functions[
            index == 0 ? functions.Count - 1 : index - 1
            ];
        type = res;
        return res;
    }
}
