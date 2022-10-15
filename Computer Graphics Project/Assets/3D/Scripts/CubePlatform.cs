using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePlatform : MonoBehaviour
{
    private PuzzleCube cube;
    private Color highlightedColor = Color.yellow;
    private Color defaultColor;
    private MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        cube = this.GetComponentInChildren<PuzzleCube>();
        meshRenderer = this.GetComponent<MeshRenderer>();
        defaultColor = meshRenderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void highlight()
    {
        meshRenderer.material.color = highlightedColor;
    }

    public Boolean hasCube()
    {
        if (cube == null) return false;
        return true;
    }

}
