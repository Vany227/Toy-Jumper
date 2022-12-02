using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Controller : MonoBehaviour
{
    public GameController gameController;

    public Rigidbody2D rb;
    RigidbodyConstraints pos;
    public float jump_height;
    public Collider2D wall;
    private bool onWall = false;
    private float wallDirection;
    private bool onGround = false;
    private float dirX;
    private float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //gameController = GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.in3dState == true)
        {
            pos = RigidbodyConstraints.FreezeRotation;
            pos = RigidbodyConstraints.FreezePosition;
        }
        else
        {
            dirX = Input.GetAxisRaw("Horizontal");
            if (!onWall)
            {
                rb.velocity = new Vector2(dirX * speed, rb.velocity.y);
            }
            else if (onWall && onGround)
            {
                rb.velocity = new Vector2(dirX * speed, rb.velocity.y);
            }
            else if (onWall && (wallDirection > 0 && dirX < 0))
            {
                rb.velocity = new Vector2(dirX * speed, rb.velocity.y);
            }
            else if (onWall && (wallDirection < 0 && dirX > 0))
            {
                rb.velocity = new Vector2(dirX * speed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            if (Input.GetButtonDown("Jump") && onGround)
            {
                rb.velocity = new Vector2(rb.velocity.x, jump_height);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision enter");
        if (collision.gameObject.name == "Ground")
        {
            
            onGround = true;
        }

        if (collision.gameObject.name == "walls")
        {
            onWall = true;
            wallDirection = dirX;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("collision exit");
        if (collision.gameObject.name == "Ground")
        {
            onGround = false;
        }

        if (collision.gameObject.name == "walls")
        {
            onWall = false;
        }
    }
}
