using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private PlatformPuzzle puzzle;
    private Vector3 dragOrigin;
    [SerializeField]
    private int minY = 1;
    [SerializeField]
    private int maxY = 8;
    private int movespeed = 4;
    Boolean zoomingOnCube = false;
    Boolean rotatingCamera = false;
    Boolean zoomingOutOfCube = false; 
    PerspectiveToOrtho perspectiveToOrtho;
    Vector3 previous3DPosition;
    Quaternion previousRotation;
    Matrix4x4 orthoMatrix;
    Matrix4x4 perspectiveMatrix;

    
    private void Start()
    {
        perspectiveToOrtho = GetComponent<PerspectiveToOrtho>();
    }

    void Update()
    {
        if (zoomingOutOfCube) zoomOut();
        if (zoomingOnCube) zoomIn();
        if (zoomingOnCube || rotatingCamera) return;
        //if (GameController.Instance.in3dState) panCamera();
    }

    public void switchTo2d()
    {
        //Save position so we can go back to it when we switch out of 2d back to 3d
        previous3DPosition = transform.position;
        previousRotation = transform.rotation;
        zoomingOnCube = true;
        rotatingCamera = true;
        StartCoroutine(rotateCamera());
        setOrthographic();
    }

    public void switchTo3d()
    {
        setPerspective();
        zoomingOutOfCube = true;
    }

    private void zoomOut()
    {
        transform.position = Vector3.MoveTowards(transform.position, previous3DPosition, movespeed * Time.deltaTime);
        transform.LookAt(puzzle.transform);
        if (Vector3.Distance(previous3DPosition, this.transform.position) < 0.05)
        {
            PuzzleCube.canClick = true;
            GameController.Instance.in3dState = true;
            zoomingOutOfCube = false;
            transform.rotation = previousRotation;
        } 
    }

    //Zoom camera in from 3d to be above cube
    private void zoomIn()
    {
        if (PuzzleCube.selectedCube != null)
        {
            Vector3 targetPosition = PuzzleCube.selectedCube.transform.position + new Vector3(0, 0, -3f);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition,movespeed * Time.deltaTime);    
            
            if (Vector3.Distance(targetPosition, this.transform.position) < 0.05)
            {
                zoomingOnCube = false;      
            }
        }
    }

    private static float eulerToDegree(float angle)
    {
        angle %= 360;
        if (angle > 180)
            return angle - 360;
        return angle;
    }

    IEnumerator rotateCamera()
    {
        //default rotation of puzzle is (90, 0, -180)
        Vector3 targetRot = new Vector3(180, eulerToDegree(PuzzleCube.selectedCube.transform.eulerAngles.y), -180);
        while (true)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRot), 2f * Time.deltaTime);
            yield return null;
            if (eulerToDegree(transform.eulerAngles.z) == 0) break;
        }
    }

    //Capture projection matrices for perspective/ortho change
    private void setOrthographic()
    {
        Camera cam = GetComponent<Camera>();
        perspectiveMatrix = cam.projectionMatrix;
        cam.orthographic = true;
        orthoMatrix = cam.projectionMatrix;
        cam.orthographic = false;
        perspectiveToOrtho.BlendToMatrix(orthoMatrix, 1);
        rotatingCamera = false;
    }

    private void setPerspective()
    {
        perspectiveToOrtho.BlendToMatrix(perspectiveMatrix, 1);
    }

    
    //Panning/Rotation for camera
    private void panCamera()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }
        if (!Input.GetMouseButton(0)) return;

        if (Input.GetAxis("Mouse Y") != 0)
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            Vector3 move = new Vector3(0, pos.y * -1, 0);


            transform.Translate(move, Space.World);
            float yP = transform.position.y;
            yP = Math.Min(yP, maxY);
            yP = Math.Max(yP, minY);
            transform.position = new Vector3(transform.position.x, yP, transform.position.z);
            transform.LookAt(puzzle.transform);
        }
    }
}
