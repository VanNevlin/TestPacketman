using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderSystem : MonoBehaviour
{
    private float vertical;
    public float climbspeed;
    public bool isLadder;
    public bool isClimbing;
    public bool canClimb;


    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        vertical = Input.GetAxis("Vertical"); //Pressing W or S


        if (isLadder && Mathf.Abs(vertical) > 0f && canClimb)
        {
            isClimbing = true;
            rb.gravityScale = 0f;
        }

    }

    private void FixedUpdate()
    {
        if (isClimbing)
        {

            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, vertical * climbspeed);


        }

        else
        {
            rb.gravityScale = 0f;
        }
        if (rb.velocity.y == 0)
        {
            isClimbing = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
        }
    }
}
