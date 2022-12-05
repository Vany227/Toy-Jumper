using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraControl : MonoBehaviour
{
    private int gridPosX;
    private int gridPosY;
    private Matrix4x4 orthographic, perspective;
    private bool orthoOn;
    private Camera cam;

    public Grid grid;
    public Transform Character;
    public Transform screen;
    private MatrixBlender blender;
    // Start is called before the first frame update
    void Start()
    {
        gridPosX = 0;
        gridPosY = 0;
        cam = GetComponent<Camera>();
        orthographic = Matrix4x4.Ortho(-16, 16, -16, 16, 0, 100);
        perspective = Matrix4x4.Perspective(101, (float)Screen.width / (float)Screen.height, 0, 100);
        cam.projectionMatrix = orthographic;
        orthoOn = true;
        blender = (MatrixBlender)GetComponent(typeof(MatrixBlender));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            orthoOn = !orthoOn;
            if (orthoOn)
                blender.BlendToMatrix(orthographic, 1f);
            else
                blender.BlendToMatrix(perspective, 1f);
        }
    }

    public void UpdateScreen()
    {

        Transform currentScreen = Character.GetComponent<Character_Controller>().currentScreen;
        Vector3 newPos = grid.CellToWorld(new Vector3Int(currentScreen.GetComponent<ScreenController>().GridX, currentScreen.GetComponent<ScreenController>().GridY, -30));
        newPos.Set(newPos.x + 16, newPos.y + 16, -30);
        this.transform.position = newPos;
        
    }
}
