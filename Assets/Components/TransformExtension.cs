using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtension
{
    public static List<GameObject> GetChilds(this Transform transform, Predicate<Transform> condition)
    {
        if (condition == null)
            condition = (t) => true;
        var count = transform.childCount;
        var res = new List<GameObject>();
        for(int i = 0; i < count; i++)
        {
            var temp = transform.GetChild(i);
            if (condition(temp)) res.Add(temp.gameObject);
        }

        return res;
    }
    public static void SetPos(this GameObject gObj, Vector2 pos)
    {
        gObj.transform.position = pos;
    }
    public static void SetPos(this Transform transform, Vector2 pos)
    {
        transform.position = pos;
    }
}
