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
    /// </summary>
    [SerializeField]
    List<PlatformRow> platforms = new List<PlatformRow>();

    static private GameObject currentPuzzle;

    [SerializeField]
    private float RotationSpeed = 1.5f;

    private Vector3 pivot;

    private void Start()
    {
        pivot = transform.GetChild(0).transform.position;
        Vector3 offset = transform.position - pivot;
        foreach (Transform child in transform)
            child.transform.position += offset;
        transform.position = pivot;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject != currentPuzzle) unhighlightPuzzle();
        if (Input.GetMouseButton(0))
        {
            transform.Rotate(0, (Input.GetAxis("Mouse X") * -RotationSpeed), 0, Space.World);
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
                if (platforms[r].row[c].Equals(platform))
                {
                    if (r - 1 >= 0 && !platforms[r - 1].row[c].hasCube()) platforms[r - 1].row[c].highlight();
                    if (r + 1 < platforms.Count && !platforms[r + 1].row[c].hasCube()) platforms[r + 1].row[c].highlight();
                    if (c - 1 >= 0 && !platforms[r].row[c-1].hasCube()) platforms[r].row[c-1].highlight();
                    if (c + 1 < platforms[r].row.Count && !platforms[r].row[c+1].hasCube()) platforms[r].row[c+1].highlight();
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
                if (platforms[r].row[c].Equals(platform))
                {
                    if (r - 1 >= 0 && platforms[r - 1].row[c].hasCube() && platforms[r - 1].row[c].cube.checkIfSelected()) // down
                        platforms[r - 1].row[c].cube.moveCube(platform.transform.position.x, platform.transform.position.y + .35f, platform.transform.position.z, platform);
                    if (r + 1 < platforms.Count && platforms[r + 1].row[c].hasCube() && platforms[r + 1].row[c].cube.checkIfSelected()) //up
                        platforms[r + 1].row[c].cube.moveCube(platform.transform.position.x, platform.transform.position.y + .35f, platform.transform.position.z, platform); 
                    if (c - 1 >= 0 && platforms[r].row[c - 1].hasCube() && platforms[r].row[c - 1].cube.checkIfSelected()) //right
                        platforms[r].row[c - 1].cube.moveCube(platform.transform.position.x, platform.transform.position.y + .35f, platform.transform.position.z, platform); 
                    if (c + 1 < platforms[r].row.Count && platforms[r].row[c + 1].hasCube() && platforms[r].row[c + 1].cube.checkIfSelected()) //left
                        platforms[r].row[c + 1].cube.moveCube(platform.transform.position.x, platform.transform.position.y + .35f, platform.transform.position.z, platform);
                }
            }
        }
    }

    public void unhighlightPuzzle()
    {
        foreach(PlatformRow row in platforms)
        {
            for(int i = 0; i < row.row.Count; i++)
            {
                row.row[i].unhighlight();
            }
        }
    }
}
