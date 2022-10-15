using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCube : MonoBehaviour
{
    [SerializeField]
    private Color selectedColor = Color.cyan;

    private Color defaultColor;
    private MeshRenderer meshRenderer;
    private PlatformPuzzle platformPuzzle;
    private CubePlatform cubePlatform;

    static private GameObject selectedCube = null;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = this.GetComponent<MeshRenderer>();
        defaultColor = meshRenderer.material.color;
        cubePlatform = this.GetComponentInParent<CubePlatform>();
        platformPuzzle = cubePlatform.GetComponentInParent<PlatformPuzzle>();
    }

    // Update is called once per frame
    void Update()
    {
        checkIfSelected();
    }

    private void checkIfSelected()
    {
        if (this.gameObject != selectedCube)
        {
            meshRenderer.material.color = defaultColor;
        }
    }

    private void OnMouseDown()
    {
        meshRenderer.material.color = selectedColor;
        selectedCube = this.gameObject;
        platformPuzzle.highlightAdjacentPlatforms(cubePlatform);
    }
}
