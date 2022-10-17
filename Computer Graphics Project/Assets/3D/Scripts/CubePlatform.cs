using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePlatform : MonoBehaviour
{
    public PuzzleCube cube;
    private Color highlightedColor = Color.yellow;
    private Color defaultColor;
    private MeshRenderer meshRenderer;
    private Boolean isHighlighted;
    private PlatformPuzzle platformPuzzle;

    // Start is called before the first frame update
    void Start()
    {
        cube = this.GetComponentInChildren<PuzzleCube>();
        platformPuzzle = this.GetComponentInParent<PlatformPuzzle>();
        meshRenderer = this.GetComponent<MeshRenderer>();
        defaultColor = meshRenderer.material.color;
        
    }

    // Update is called once per frame
    void Update()
    {
        cube = this.GetComponentInChildren<PuzzleCube>();
    }

    public void highlight()
    {
        meshRenderer.material.color = highlightedColor;
        isHighlighted = true;
    }

    public void unhighlight()
    {
        meshRenderer.material.color = defaultColor;
        isHighlighted = false;
    }

    public Boolean hasCube()
    {
        if (cube == null) return false;
        return true;
    }

    private void OnMouseDown()
    {
        if (isHighlighted)
        {
            platformPuzzle.checkForSelectedCube(this);
        }
    }
}
