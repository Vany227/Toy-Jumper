using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class PlatformRow
{
    public List<CubePlatform> row;
}

public class PlatformPuzzle : MonoBehaviour
{
    [SerializeField]
    List<PlatformRow> platforms = new List<PlatformRow>();
    static private GameObject currentPuzzle;
    [SerializeField]
    private float RotationSpeed = 1.5f;
    private Vector3 pivot;
    Quaternion startRotation;

    private void Start()
    {
        pivot = transform.GetChild(0).transform.position;
        Vector3 offset = transform.position - pivot;
        foreach (Transform child in transform)
            child.transform.position += offset;
        transform.position = pivot;
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameController.Instance.in3dState) return;
        if (this.gameObject != currentPuzzle) unhighlightPuzzle();
        if (Input.GetMouseButton(0))
        {
            //transform.Rotate(0, 0, (Input.GetAxis("Mouse X") * -RotationSpeed), Space.World); //rotate puzzle
        }
    }

    /// <summary>
    /// Unhighlights all platforms and then searches for the given cubePlatform to highlight its neighbors
    /// </summary>
    /// <param name="platform"></param>
    public void highlightAdjacentPlatforms(CubePlatform platform)
    {
        currentPuzzle = this.gameObject;
        unhighlightPuzzle();
        for(int r = 0; r < platforms.Count; r ++)
        {
            for(int c = 0; c < platforms[r].row.Count; c++)
            {
                if (platforms[r].row[c] == null) continue;
                if (platforms[r].row[c].Equals(platform))
                {
                    if (r - 1 >= 0 && platforms[r - 1].row[c]!= null && !platforms[r - 1].row[c].hasCube())
                        platforms[r - 1].row[c].highlight();
                    if (r + 1 < platforms.Count && platforms[r + 1].row[c] != null && !platforms[r + 1].row[c].hasCube()) platforms[r + 1].row[c].highlight();
                    if (c - 1 >= 0 && platforms[r].row[c-1] != null && !platforms[r].row[c-1].hasCube()) platforms[r].row[c-1].highlight();
                    if (c + 1 < platforms[r].row.Count && platforms[r].row[c+1] != null && !platforms[r].row[c+1].hasCube()) platforms[r].row[c+1].highlight();
                }
            }
        }
    }

    public void checkForSelectedCube(CubePlatform platform)
    {
        currentPuzzle = this.gameObject;
        unhighlightPuzzle();
        for (int r = 0; r < platforms.Count; r++)
        {
            for (int c = 0; c < platforms[r].row.Count; c++)
            {
                if (platforms[r].row[c] == null) continue;
                if (platforms[r].row[c].Equals(platform))
                {
                    if (r - 1 >= 0 && platforms[r - 1].row[c] != null && platforms[r - 1].row[c].hasCube() && platforms[r - 1].row[c].cube.checkIfSelected()) // down
                        platforms[r - 1].row[c].cube.moveCube(platform.transform.position.x, platform.transform.position.y, platform.transform.position.z - .35f, platform);
                    if (r + 1 < platforms.Count && platforms[r + 1].row[c] != null && platforms[r + 1].row[c].hasCube() && platforms[r + 1].row[c].cube.checkIfSelected()) //up
                        platforms[r + 1].row[c].cube.moveCube(platform.transform.position.x, platform.transform.position.y, platform.transform.position.z - .35f, platform); 
                    if (c - 1 >= 0 && platforms[r].row[c - 1] != null && platforms[r].row[c - 1].hasCube() && platforms[r].row[c - 1].cube.checkIfSelected()) //right
                        platforms[r].row[c - 1].cube.moveCube(platform.transform.position.x, platform.transform.position.y, platform.transform.position.z - .35f, platform); 
                    if (c + 1 < platforms[r].row.Count && platforms[r].row[c + 1] != null && platforms[r].row[c + 1].hasCube() && platforms[r].row[c + 1].cube.checkIfSelected()) //left
                        platforms[r].row[c + 1].cube.moveCube(platform.transform.position.x, platform.transform.position.y, platform.transform.position.z - .35f, platform);
                }
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

    public IEnumerator rotateIt()
    {
        //Vector3 targetRot = new Vector3(0, 0, eulerToDegree(PuzzleCube.selectedCube.transform.eulerAngles.x));
        Vector3 targetRot = new Vector3(90, 180, 0);
        while (true)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRot), 2f * Time.deltaTime);
            yield return null;
            //if (eulerToDegree(transform.eulerAngles.z) == 0) break;
            if (eulerToDegree(transform.eulerAngles.z) == 0) break;
        }
        //GameController.Instance.turnOn2dPhysics();
    }

    public void unhighlightPuzzle()
    {
        foreach(PlatformRow row in platforms)
        {
            for(int i = 0; i < row.row.Count; i++)
            {
                if(row.row[i] != null)  row.row[i].unhighlight();
            }
        }
    }
}
