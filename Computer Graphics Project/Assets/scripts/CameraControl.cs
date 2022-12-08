using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraControl : MonoBehaviour
{
    private int gridPosX;
    private int gridPosY;
    private Matrix4x4 orthographic, perspective;
    public bool orthoOn;
    private bool rotating;
    private bool moving;
    private Camera cam;
    public Transform orthographicTransform, perspectiveTransform;
    public Vector3 originalPosition;
    private Vector3 lookatPosition;

    public Grid grid;
    public Transform Character;
    public Transform screen;
    private MatrixBlender blender;
    // Start is called before the first frame update
    void Start()
    {
        



    }

    // Update is called once per frame
    void Update()
    {
        if (!orthoOn && Input.GetMouseButton(0) && !moving && !rotating)
        {
            transform.RotateAround(lookatPosition, new Vector3((Input.GetAxis("Mouse Y") * -3f), -(Input.GetAxis("Mouse X") * -3f), 0.0f), 20f * Time.deltaTime * 8f);
            perspectiveTransform.position = transform.position;
            perspectiveTransform.rotation = transform.rotation;
        }
        if (!orthoOn && Input.GetKeyDown(KeyCode.F) && !moving && !rotating)
        {
            transform.position = originalPosition;
            transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }

        if (Input.GetKeyDown(KeyCode.Q) && !moving && !rotating)
        {
            orthoOn = !orthoOn;
            if (orthoOn)
            {

                Vector3 position = this.GetComponent<Transform>().position;
                perspectiveTransform.position = position;
                Quaternion rotation = this.GetComponent<Transform>().rotation;
                perspectiveTransform.rotation = rotation;
                blender.BlendToMatrix(orthographic, 1f);
                StartCoroutine(rotateCamera(orthographicTransform.rotation));
                StartCoroutine(panCamera(orthographicTransform.position, 4f));
            }
            else
            {
                blender.BlendToMatrix(perspective, 1f);
                StartCoroutine(rotateCamera(perspectiveTransform.rotation));
                StartCoroutine(panCamera(perspectiveTransform.position, 4f));
            }
                
        }
        
    }

    public void UpdateOrthoScreen()
    {

        Transform currentScreen = Character.GetComponent<Character_Controller>().currentScreen;
        Vector3 newPos = grid.CellToWorld(new Vector3Int(currentScreen.GetComponent<ScreenController>().GridX, currentScreen.GetComponent<ScreenController>().GridY, -30));
        newPos.Set(newPos.x + 16, newPos.y + 16, -30);
        orthographicTransform.position = newPos;
        
    }

    IEnumerator rotateCamera(Quaternion goal)
    {
        rotating = true;
        while (true)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, goal, 4f * Time.deltaTime);
            yield return null;
            if (Quaternion.Angle(transform.rotation, goal) <= 0.00001f)
            {
                rotating = false;
                break;
            }
        }
    }

    public IEnumerator panCamera(Vector3 goal, float speed)
    {
        moving = true;
        while (true)
        {
            transform.position = Vector3.Lerp(transform.position, goal, speed * Time.deltaTime);
            yield return null;
            if (Vector3.Distance(transform.position, goal) <= 0.01f)
            {
                moving = false;
                break;
            }
        }
    }
    
    public void setUpPerspective2X2()
    {
        orthographicTransform = new GameObject().transform;
        perspectiveTransform = new GameObject().transform;
        gridPosX = 0;
        gridPosY = 0;
        cam = GetComponent<Camera>();
        orthographic = Matrix4x4.Ortho(-16, 16, -16, 16, 0, 100);
        perspective = Matrix4x4.Perspective(90, (float)Screen.width / (float)Screen.height, 0.3f, 300f);
        cam.projectionMatrix = perspective;
        orthoOn = false;
        blender = (MatrixBlender)GetComponent(typeof(MatrixBlender));
        orthographicTransform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        Vector3 newPos = grid.CellToWorld(new Vector3Int(1, 1, -30));
        newPos = new Vector3(newPos.x - grid.cellGap.x / 2 + 1, newPos.y - grid.cellGap.y / 2 + 1, -75);
        lookatPosition = new Vector3(newPos.x, newPos.y, 41.5f);
        transform.position = newPos;
        originalPosition = newPos;
    }
    public void setUpPerspective3X3()
    {
        orthographicTransform = new GameObject().transform;
        perspectiveTransform = new GameObject().transform;
        gridPosX = 0;
        gridPosY = 0;
        cam = GetComponent<Camera>();
        orthographic = Matrix4x4.Ortho(-16, 16, -16, 16, 0, 100);
        perspective = Matrix4x4.Perspective(90, (float)Screen.width / (float)Screen.height, 0.3f, 300f);
        cam.projectionMatrix = perspective;
        orthoOn = false;
        blender = (MatrixBlender)GetComponent(typeof(MatrixBlender));
        orthographicTransform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        Vector3 newPos = grid.CellToWorld(new Vector3Int(1, 1, -30));
        newPos = new Vector3(newPos.x + grid.cellSize.x / 2 + 1, newPos.y + grid.cellSize.y / 2 + 1, -75);
        lookatPosition = new Vector3(newPos.x, newPos.y, 41.5f);
        transform.position = newPos;
        originalPosition = newPos;
    }
}
