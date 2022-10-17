using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject != currentPuzzle) unhighlightPuzzle();
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
                        platforms[r - 1].row[c].cube.moveCube(platform.transform.position.x, platform.transform.position.y + .5f, platform.transform.position.z, platform);
                    if (r + 1 < platforms.Count && platforms[r + 1].row[c].hasCube() && platforms[r + 1].row[c].cube.checkIfSelected()) //up
                        platforms[r + 1].row[c].cube.moveCube(platform.transform.position.x, platform.transform.position.y + .5f, platform.transform.position.z, platform); 
                    if (c - 1 >= 0 && platforms[r].row[c - 1].hasCube() && platforms[r].row[c - 1].cube.checkIfSelected()) //right
                        platforms[r].row[c - 1].cube.moveCube(platform.transform.position.x, platform.transform.position.y + .5f, platform.transform.position.z, platform); 
                    if (c + 1 < platforms[r].row.Count && platforms[r].row[c + 1].hasCube() && platforms[r].row[c + 1].cube.checkIfSelected()) //left
                        platforms[r].row[c + 1].cube.moveCube(platform.transform.position.x, platform.transform.position.y + .5f, platform.transform.position.z, platform);
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
