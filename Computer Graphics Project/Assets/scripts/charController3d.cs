using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charController3d : MonoBehaviour
{
    private Rigidbody rb;
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
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
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
