using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class FuncTypeSwitchController : MonoBehaviour
{
    [SerializeField] bool isLeft;
    FuncTypeSwitch fSwitch;
    void Start()
    {
        fSwitch = transform.parent.GetComponent<FuncTypeSwitch>();
        UpdateFunctionType();
    }
    private void OnMouseDown()
    {
        UpdateFunctionType();
    }
    void UpdateFunctionType()
    {
        fSwitch.UpdateFuncType(isLeft);
    }
}
