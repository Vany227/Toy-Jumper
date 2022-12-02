using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPivot : MonoBehaviour
{
    float rotateSpeed = 3f;
    // Update is called once per frame
    void Update()
    {
        if(GameController.Instance.in3dState && Input.GetMouseButtonDown(0)) {
            transform.Rotate(0, (Input.GetAxis("Mouse X") * -rotateSpeed), 0);
        }
    }
}
