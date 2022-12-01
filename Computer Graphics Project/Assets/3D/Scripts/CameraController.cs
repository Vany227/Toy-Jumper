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
    PerspectiveToOrtho perspectiveToOrtho;

    Matrix4x4 orthoMatrix;
    Matrix4x4 perspectiveMatrix;


    private void Start()
    {
        transform.LookAt(puzzle.transform);
        perspectiveToOrtho = GetComponent<PerspectiveToOrtho>();
    }

    private void zoom()
    {
        zoomingOnCube = true;
        if (PuzzleCube.selectedCube != null)
        {
            Vector3 targetPosition = PuzzleCube.selectedCube.transform.position + new Vector3(0, 3f, 0);
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

    IEnumerator RotateImage(Action callback)
    {
        //default rotation of puzzle is (90, 0, -180)
        Vector3 targetRot = new Vector3(90, eulerToDegree(PuzzleCube.selectedCube.transform.eulerAngles.y), -180);
        //eulerToDegree(transform.eulerAngles.z) != 0
        while (true)
        {
            
            Debug.Log(eulerToDegree(transform.eulerAngles.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRot), 2f * Time.deltaTime);
            yield return null;
            if (eulerToDegree(transform.eulerAngles.z) == 0) break;
        }
        if(callback != null) setOrthographic(); 
    }

    private void setOrthographic()
    {
        Camera cam = GetComponent<Camera>();
        perspectiveMatrix = cam.projectionMatrix;
        cam.orthographic = true;
        orthoMatrix = cam.projectionMatrix;
        cam.orthographic = false;
        perspectiveToOrtho.BlendToMatrix(orthoMatrix, 1);
    }

    Boolean rotateCamera = false;


    void Update()
    {
        //Moving camera for 2d switch
        if (Input.GetKeyDown(KeyCode.E))
        {
            zoomingOnCube = true;
            rotateCamera = true;
            PuzzleCube.canClick = false;
        }

        if (rotateCamera)
        {
            rotateCamera = false;
            StartCoroutine(RotateImage(setOrthographic));
        }
        
        if (zoomingOnCube) zoom();


        //Panning/Rotation
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(0)) return;

        if (!zoomingOnCube && Input.GetAxis("Mouse Y") != 0)
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            Vector3 move = new Vector3(0, pos.y * -1,0);


            transform.Translate(move, Space.World);
            float yP = transform.position.y;
            yP = Math.Min(yP, maxY);
            yP = Math.Max(yP, minY);
            transform.position = new Vector3(transform.position.x, yP, transform.position.z);
            transform.LookAt(puzzle.transform);
        }
    }
}
