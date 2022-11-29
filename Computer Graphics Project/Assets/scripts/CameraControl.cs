using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraControl : MonoBehaviour
{
    private int gridPosX;
    private int gridPosY;
    private Camera cam;


    public Transform screen;
    // Start is called before the first frame update
    void Start()
    {
        gridPosX = 0;
        gridPosY = 0;
        cam = GetComponent<Camera>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
