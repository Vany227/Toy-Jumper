using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Character_Controller : MonoBehaviour
{

    private Rigidbody2D rb;
    public float jump_height;
    public Collider2D wall;
    private bool onWall = false;
    private float wallDirection;
    private bool onGround = false;
    private float dirX;
    private bool onTrap = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        if (!onWall)
        {
            rb.velocity = new Vector2(dirX * 7f, rb.velocity.y);
        }
        else if (onWall && onGround)
        {
            rb.velocity = new Vector2(dirX * 7f, rb.velocity.y);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            
            onGround = true;
            Debug.Log(onGround);
        }

        if (collision.gameObject.name == "walls")
        {
            onWall = true;
            wallDirection = dirX;
            Debug.Log(wallDirection);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = false;
        }

        if (collision.gameObject.name == "walls")
        {
            onWall = false;
        }
    }
}
