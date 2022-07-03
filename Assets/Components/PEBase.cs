using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEBase : MonoBehaviour
{
    [SerializeField] PEPartBuilder builder;
    List<Vector3> defaultPositions = new List<Vector3>()
    {
        new Vector3(-0.5f,0f,-0.1f),
        new Vector3(-0.5f,0.5f,-0.1f),
        new Vector3(0f,0.5f,-0.1f),
        new Vector3(0.5f,0f,-0.1f),
        new Vector3(0.5f,-0.5f,-0.1f),
        new Vector3(0f,-0.5f,-0.1f),
        new Vector3(-0.5f,-0.5f,-0.1f),
        new Vector3(0.5f,0.5f,-0.1f)
    };

    public void LoadNodes(int inCount, int outCount)
    {
        for (int i = 0; i < inCount; i++)
            builder.CreateInNodePos(defaultPositions[i], transform, i + 1);
        for (int i = 0; i < outCount; i++)
            builder.CreateOutNodePos(defaultPositions[i+inCount], transform, i + 1);
    }
}
