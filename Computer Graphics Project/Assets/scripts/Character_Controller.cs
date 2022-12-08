using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Xml;

public class Character_Controller : MonoBehaviour
{
    private Rigidbody2D rb;
    public Animator animator;
    public float jump_height;
    public Collider2D wall;
    private bool onWall = false;
    private float wallDirection;
    private bool onGround = false;
    private float dirX;
    public Transform currentScreen;
    public Grid GameGrid;
    public Transform Camera;
    GameController gameController;
    CameraControl cameraControl;
    public bool isFacingLeft;
    public bool spawnFacingLeft;
    public bool dead = false;
    private Vector2 facingLeft;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameGrid.GetComponent<GameController>();
        cameraControl = Camera.GetComponent<CameraControl>();
        transform.parent = currentScreen;
        rb = GetComponent<Rigidbody2D>();
        facingLeft = new Vector2(-transform.localScale.x, transform.localScale.y);
        if (spawnFacingLeft)
        {
            transform.localScale = facingLeft;
            isFacingLeft = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent = currentScreen;
        dirX = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(dirX));
        if (dirX > 0 && isFacingLeft)
        {
            isFacingLeft = false;
            Flip();
        }
        if (dirX < 0 && !isFacingLeft)
        {
            isFacingLeft = true;
            Flip();
        }
        if (cameraControl.orthoOn)
        {
            if (!onWall)
            {
                rb.velocity = new Vector2(dirX * 7f, rb.velocity.y);
            }
            else if (onWall && onGround)
            {
                rb.velocity = new Vector2(dirX * 7f, rb.velocity.y);
            }
            else if (onWall && (wallDirection > 0 && dirX < 0))
            {
                rb.velocity = new Vector2(dirX * 7f, rb.velocity.y);
            }
            else if (onWall && (wallDirection < 0 && dirX > 0))
            {
                rb.velocity = new Vector2(dirX * 7f, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            if (Input.GetButtonDown("Jump") && onGround)
            {
                animator.SetBool("isJumping", true);
                rb.velocity = new Vector2(rb.velocity.x, jump_height);
            }
        } 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ground")
        {
            animator.SetBool("isJumping", false);
            onGround = true;
        }

        if (collision.gameObject.name == "walls")
        {
            onWall = true;
            wallDirection = dirX;
        }
        if (collision.gameObject.name == "keys")
        {
            Destroy(collision.gameObject);
            if(--gameController.numKeys == 0) {
                SceneManager.LoadScene("Title Screen");
            }
        }
        if(collision.gameObject.name == "traps" && dead == false)
        {
            dead = true;
            animator.SetBool("isJumping", false);
            animator.SetTrigger("isDead");
        }
        if (!collision.gameObject.GetComponent<TilemapRenderer>().enabled)
        {
            Transform[,] game_matrix = new Transform[3, 3];
            switch (collision.gameObject.name)
            {
                case "Bottom Side":
                    game_matrix = collision.gameObject.GetComponentInParent<Transform>().GetComponentInParent<Transform>().GetComponentInParent<Transform>().GetComponentInParent<Transform>().GetComponentInParent<GameController>().game_matrix;
                    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - GameGrid.cellGap.y - 4f, this.transform.position.z);
                    this.currentScreen = game_matrix[this.currentScreen.GetComponent<ScreenController>().GridX, this.currentScreen.GetComponent<ScreenController>().GridY - 1];
                    Camera.GetComponent<CameraControl>().UpdateOrthoScreen();
                    StartCoroutine(Camera.GetComponent<CameraControl>().panCamera(Camera.GetComponent<CameraControl>().orthographicTransform.position, 10f));
                    break;
                case "Top Side":
                    game_matrix = collision.gameObject.GetComponentInParent<Transform>().GetComponentInParent<Transform>().GetComponentInParent<Transform>().GetComponentInParent<Transform>().GetComponentInParent<GameController>().game_matrix;
                    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + GameGrid.cellGap.y + 3.1f, this.transform.position.z);
                    this.currentScreen = game_matrix[this.currentScreen.GetComponent<ScreenController>().GridX, this.currentScreen.GetComponent<ScreenController>().GridY + 1];
                    Camera.GetComponent<CameraControl>().UpdateOrthoScreen();
                    Debug.Log("swag");
                    StartCoroutine(Camera.GetComponent<CameraControl>().panCamera(Camera.GetComponent<CameraControl>().orthographicTransform.position, 10f));
                    break;
                case "Left Side":
                    game_matrix = collision.gameObject.GetComponentInParent<Transform>().GetComponentInParent<Transform>().GetComponentInParent<Transform>().GetComponentInParent<Transform>().GetComponentInParent<GameController>().game_matrix;
                    this.transform.position = new Vector3(this.transform.position.x - GameGrid.cellGap.x - 3.1f, this.transform.position.y, this.transform.position.z);
                    this.currentScreen = game_matrix[this.currentScreen.GetComponent<ScreenController>().GridX - 1, this.currentScreen.GetComponent<ScreenController>().GridY];
                    Camera.GetComponent<CameraControl>().UpdateOrthoScreen();
                    StartCoroutine(Camera.GetComponent<CameraControl>().panCamera(Camera.GetComponent<CameraControl>().orthographicTransform.position, 10f));
                    break;
                case "Right Side":
                    game_matrix = collision.gameObject.GetComponentInParent<Transform>().GetComponentInParent<Transform>().GetComponentInParent<Transform>().GetComponentInParent<Transform>().GetComponentInParent<GameController>().game_matrix;
                    this.transform.position = new Vector3(this.transform.position.x + GameGrid.cellGap.x + 3.1f, this.transform.position.y, this.transform.position.z);
                    this.currentScreen = game_matrix[this.currentScreen.GetComponent<ScreenController>().GridX + 1, this.currentScreen.GetComponent<ScreenController>().GridY];
                    Camera.GetComponent<CameraControl>().UpdateOrthoScreen();
                    StartCoroutine(Camera.GetComponent<CameraControl>().panCamera(Camera.GetComponent<CameraControl>().orthographicTransform.position, 10f));
                    break;
            }
        }
        



    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ground")
        {
            onGround = false;
        }

        if (collision.gameObject.name == "walls")
        {
            onWall = false;
        }
    }

    protected virtual void Flip()
    {
        if (isFacingLeft)
        {
            transform.localScale = facingLeft;
        }
        if (!isFacingLeft)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }

    }

    public void deathReset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
