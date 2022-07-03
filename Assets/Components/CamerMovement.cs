using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3? _clickPos;
    Vector3 _camPos;
    Vector3 mousePos => Camera.main.ScreenToWorldPoint(Input.mousePosition);

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse2))
        {
            if (_clickPos == null)
            {
                _clickPos = mousePos;
                _camPos = transform.position;
            }
            transform.position = _camPos + (Vector3)((_clickPos.Value - mousePos) * 0.5f);
        }
        else
            _clickPos = null;

        if(Input.mouseScrollDelta != Vector2.zero)
        {
            Camera.main.orthographicSize -= Input.mouseScrollDelta.y * 0.3f;
        }
    }
}
