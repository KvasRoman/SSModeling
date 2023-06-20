using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField]
    LineRenderer lineRenderer;
    [SerializeField]
    float speed;

    List<Vector3> values = new List<Vector3>();
    float timer = 0.15f;
    double remeaning;

    int innerCounter = 0;
    Func<float> innerFunc;

    Rigidbody2D rb2d;
    private void Start()
    {
        remeaning = timer;
        lineRenderer = transform.GetChilds((c) => c.GetComponent<LineRenderer>() != null)[0].GetComponent<LineRenderer>();

        

        rb2d = lineRenderer.transform.GetComponent<Rigidbody2D>();
        rb2d.velocity = new Vector3((-0.1f / timer + 0.041f) * transform.localScale.x
            , 0, 0);
    }
    void FixedUpdate()
    {
        remeaning -= Time.deltaTime;
        //if(false)
        if(remeaning < 0)
        {
            values.Add(new Vector3(innerCounter * 1f / 10f + 0.4f,2 * Mathf.Abs(Mathf.Sin(innerCounter++ * 1f / 10f)), 0f));
            if (values.Count > 80)
            {
                values.RemoveAt(0);
            }
            lineRenderer.positionCount = values.Count;
            lineRenderer.SetPositions(values.ToArray());
            
            remeaning = timer;

        }
    }
    void SetDisplayFunction(Func<float> func)
    {

    }
}
