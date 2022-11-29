using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.Device;

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
            Vector3 targetPosition = PuzzleCube.selectedCube.transform.position + new Vector3(0, 0.7f, 0);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition,movespeed * Time.deltaTime);
            transform.LookAt(PuzzleCube.selectedCube.transform);
            
            
            if (Vector3.Distance(targetPosition, this.transform.position) < 0.3)
            {
                zoomingOnCube = false;
                //SceneManager.LoadScene("Title Screen");          
            }
        }
    }


    void Update()
    {
        //Moving camera for 2d switch
        if (Input.GetKeyDown(KeyCode.Space))
        {
            zoomingOnCube = true;
            PuzzleCube.canClick = false;

            //Start fade to black
            Color fixedColor = Color.black;
            fixedColor.a = 1;
            image.color = fixedColor;
            image.CrossFadeAlpha(0f, 0f, true);
            image.CrossFadeAlpha(1f, 2.1f, false);
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
