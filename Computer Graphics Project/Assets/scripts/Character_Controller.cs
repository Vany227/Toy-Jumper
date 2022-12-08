using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public bool isFacingLeft;
    public bool spawnFacingLeft;
    private Vector2 facingLeft;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        facingLeft = new Vector2(-transform.localScale.x, transform.localScale.y);
        if (spawnFacingLeft) {
            transform.localScale = facingLeft;
            isFacingLeft = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("Speed",Mathf.Abs(dirX));
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
            animator.SetBool("isJumping", true);
            rb.velocity = new Vector2(rb.velocity.x, jump_height);
        }
    }

    protected virtual void Flip() {
        if (isFacingLeft) {
            transform.localScale = facingLeft;
        }
        if (!isFacingLeft) {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    
    }
    private void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {

            animator.SetBool("isJumping", false);
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
