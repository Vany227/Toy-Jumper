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
    private int movespeed = 3;
    Boolean zoomingOnCube = false;

    [SerializeField]
    private Image image;
    

    private void Start()
    {
        transform.LookAt(puzzle.transform);
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


    IEnumerator RotateImage()
    {
        //default rotation of puzzle is (90, 0, -180)
        Vector3 targetRot = new Vector3(90, eulerToDegree(PuzzleCube.selectedCube.transform.eulerAngles.y), -180);
        while (Vector3.Distance(transform.eulerAngles, targetRot) > 0.01f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRot), 0.01f * Time.deltaTime);
            yield return null;
        }
        transform.rotation = Quaternion.Euler(targetRot);
        yield return null;
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

        if (rotateCamera) StartCoroutine(RotateImage());
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
