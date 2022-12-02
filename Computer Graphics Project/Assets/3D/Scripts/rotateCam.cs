using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateCam : MonoBehaviour
{
    float rotateSpeed = 2f;
    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.in3dState && Input.GetMouseButton(0))
        {
            transform.Rotate(0, (Input.GetAxis("Mouse X") * -rotateSpeed), 0);
        }
    }
}
