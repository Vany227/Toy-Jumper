using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraControl : MonoBehaviour
{
    private int gridPosX;
    private int gridPosY;
    private Matrix4x4 orthographic, perspective;
    private bool orthoOn;
    private bool rotating;
    private bool moving;
    private Camera cam;
    public Transform orthographicTransform, perspectiveTransform;
    private Vector3 lookatPosition;

    public Grid grid;
    public Transform Character;
    public Transform screen;
    private MatrixBlender blender;
    // Start is called before the first frame update
    void Start()
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



    }

    // Update is called once per frame
    void Update()
    {
        if (!orthoOn && Input.GetMouseButton(0) && !moving && !rotating)
        {

            transform.RotateAround(lookatPosition, new Vector3(0f, -(Input.GetAxis("Mouse X") * -3f), 0.0f), 20f * Time.deltaTime * 3f);
            //transform.Rotate(0, , 0);
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
                rotating = true;
                StartCoroutine(rotateCamera(orthographicTransform.rotation));
                moving = true;
                StartCoroutine(panCamera(orthographicTransform.position, 4f));
            }
            else
            {
                blender.BlendToMatrix(perspective, 1f);
                rotating = true;
                StartCoroutine(rotateCamera(perspectiveTransform.rotation));
                moving = true;
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
        while (true)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, goal, 4f * Time.deltaTime);
            yield return null;
            if (Quaternion.Angle(transform.rotation, goal) <= 0.01f)
            {
                rotating = false;
                break;
            }
        }
    }

    public IEnumerator panCamera(Vector3 goal, float speed)
    {
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
        Vector3 newPos = grid.CellToWorld(new Vector3Int(1, 1, -30));
        newPos = new Vector3(newPos.x - grid.cellGap.x / 2 + 1, newPos.y - grid.cellGap.y / 2 + 1, -75);
        lookatPosition = new Vector3(newPos.x, newPos.y, 41.5f);
        transform.LookAt(lookatPosition);
        transform.position = newPos;
    }
}
