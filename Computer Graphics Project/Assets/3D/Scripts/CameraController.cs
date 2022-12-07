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
    public Boolean rotationInProgress = false;

    
    private void Start()
    {
        perspectiveToOrtho = GetComponent<PerspectiveToOrtho>();
    }

    void Update()
    {
        if (zoomingOutOfCube) zoomOut();
        if (zoomingOnCube) zoomIn();
        //if (GameController.Instance.in3dState) panCamera();
    }

    public void switchTo2d()
    {
        rotationInProgress = true;
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
        rotationInProgress = true;
        setPerspective();
        StartCoroutine(rotateCameraOut());
        zoomingOutOfCube = true;
    }

    private void zoomOut()
    {
        transform.position = Vector3.MoveTowards(transform.position, previous3DPosition, movespeed * Time.deltaTime);
        if (Vector3.Distance(previous3DPosition, this.transform.position) < 0.05)
        {
            PuzzleCube.canClick = true;
            //GameController.Instance.in3dState = true;
            zoomingOutOfCube = false;
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

    IEnumerator rotateCameraOut()
    {
        while (true)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, previousRotation, 2f * Time.deltaTime);
            yield return null;
            if (Quaternion.Angle(transform.rotation, previousRotation) <= 0.01f) break;
        }
        rotationInProgress = false;
    }

    IEnumerator rotateCamera()
    {
        Vector3 targetRot = new Vector3(180, eulerToDegree(PuzzleCube.selectedCube.transform.eulerAngles.y), -180);
        while (true)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRot), 2f * Time.deltaTime);
            yield return null;
            if (Quaternion.Angle(transform.rotation, Quaternion.Euler(targetRot)) <= 0.01f) break;
        }
        rotationInProgress = false;
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
        perspectiveToOrtho.BlendToMatrix(perspectiveMatrix, 1.5f);
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
