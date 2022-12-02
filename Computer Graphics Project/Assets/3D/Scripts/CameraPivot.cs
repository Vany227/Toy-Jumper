using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPivot : MonoBehaviour
{
    float speed = 3f;
    CameraController cam;

    private void Start()
    {
        cam = GetComponentInChildren<CameraController>();
    }
    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.in3dState && Input.GetMouseButton(0) && !cam.rotationInProgress)
        {
            transform.Rotate(0, (Input.GetAxis("Mouse X") * -speed), 0);
        }
    }
}
