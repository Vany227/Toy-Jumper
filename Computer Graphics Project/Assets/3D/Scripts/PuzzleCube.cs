using System;
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
    public CubePlatform cubePlatform;
    private Boolean canMove;
    private Vector3 targetPosition;
    static public Boolean canClick = true;
    static public GameObject selectedCube = null;
    private float moveSpeed = .03f;
    public AudioClip woodSlideSound;
    private AudioSource blockAudio;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = this.GetComponent<MeshRenderer>();
        defaultColor = meshRenderer.material.color;
        cubePlatform = this.GetComponentInParent<CubePlatform>();
        platformPuzzle = cubePlatform.GetComponentInParent<PlatformPuzzle>();
        blockAudio = GetComponent<AudioSource>();
        woodSlideSound = Resources.Load<AudioClip>("Audio/SlideSound");
    }

    // Update is called once per frame
    void Update()
    {
        checkIfSelected();
        cubeMovement();
        
    }

    void cubeMovement()
    {
        
        if (canMove)
        {
            selectedCube.transform.position = Vector3.MoveTowards(selectedCube.transform.position, targetPosition, moveSpeed);
            
            if (selectedCube.transform.position == targetPosition)
            {
                canMove = false;
                canClick = true;
            }
            else
            {
                canClick = false;
            }
        }
    }

    public Boolean checkIfSelected()
    {
        if (this.gameObject != selectedCube)
        {
            meshRenderer.material.color = defaultColor;
            return false;
        }
        else return true;
    }

    private void OnMouseDown()
    {
        if (canClick)
        {
            meshRenderer.material.color = selectedColor;
            selectedCube = this.gameObject;
            platformPuzzle.highlightAdjacentPlatforms(cubePlatform);
        }
    }

    public void moveCube(float x, float y, float z, CubePlatform platform)
    {
        targetPosition = new Vector3(x, y, z);
        canMove = true;
        transform.parent = platform.transform;
        cubePlatform = this.GetComponentInParent<CubePlatform>();
        meshRenderer.material.color = defaultColor;
        //blockAudio.PlayOneShot(woodSlideSound, 1.0f);
    }
}
