using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float originalGravity;
    public float speed = 8f;
    public float jumpPower = 16f;
    public LadderSystem ladderSystem;

    private bool isFacingRight = true;

    public bool isActivePlayer = false; //Reference to the player-switcher

    public bool YesPress = true;
    public bool canJump = true;


    private Transform playerTransform;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask boxLayer;

    private CameraMovement cameraMovement;
    private GameManager gameManager;
    private AudioSource MovementAudio;
    private AudioSource JumpAudio;
    public Animator animator; // Nevlin Added

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        originalGravity = rb.gravityScale;
        playerTransform = transform;

        gameManager = FindObjectOfType<GameManager>();
        cameraMovement = FindObjectOfType<CameraMovement>();

        GameObject movementAudio = GameObject.Find("Walking");
        GameObject jumpAudio = GameObject.Find("Jump");

        MovementAudio = movementAudio.GetComponent<AudioSource>();
        JumpAudio = jumpAudio.GetComponent<AudioSource>();
    }

    void Update()
    {
        animator.SetFloat("PlayerSpeed", Mathf.Abs(horizontal)); // Nevlin Added

        if (!isActivePlayer) return; //*Safety*

        if (!ladderSystem.isClimbing)
        {
            horizontal = Input.GetAxisRaw("Horizontal"); //Pressing A and D
        }


        if (Input.GetButtonDown("Jump") && (IsGrounded() || IsOnBox()) && gameManager.canMove == true && canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            JumpAudio.Play();
        }

        if (rb.velocity.x > 0.5 || rb.velocity.x < -0.5)
        {
            if (!MovementAudio.isPlaying && rb.velocity.y == 0)
            {
                MovementAudio.Play();
            }

        }

        Flip();



        if (rb != null && Mathf.Abs(rb.velocity.y) > 0.01 && !ladderSystem.isClimbing)
        {
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsClimbing", false);
        }

        // if (rb != null && Mathf.Abs(rb.velocity.y) < 0.01 && !ladderSystem.isClimbing)
        // {
        //     animator.SetBool("IsJumping", false);
        // }
        if (rb != null && Mathf.Abs(rb.velocity.y) > 0.01 && ladderSystem.isClimbing)
        {
            animator.SetBool("IsClimbing", true);
            animator.SetBool("IsJumping", false);
        }

        if (rb != null && Mathf.Abs(rb.velocity.y) < 0.01)
        {
            animator.SetBool("IsClimbing", false);
            animator.SetBool("IsJumping", false);
        }


    }


    private void FixedUpdate()
    {

        if (cameraMovement.CameraTarget != playerTransform || gameManager.textUp == true)
        {
            rb.velocity = Vector2.zero;

        }

        if (!isActivePlayer)
        {
            rb.velocity = Vector2.zero; // Stop player movement when not active
            rb.gravityScale = 30f; //Boosted Gravity to compensate Vector2.Zero slowing down fall rate
            return;
        }

        else if (isActivePlayer)
        {
            rb.gravityScale = originalGravity;//*Safety*
            if (!ladderSystem.isClimbing && ladderSystem.isLadder)
            { // Nevlin Added
                rb.gravityScale = 0;
            }
        }

        if (gameManager.canMove == true)
        {

            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);


        }
    }

    private bool IsGrounded()
    {

        return Physics2D.OverlapCircle(groundCheck.position, 0.25f, groundLayer);

    }

    private bool IsOnBox()
    {

        return Physics2D.OverlapCircle(groundCheck.position, 0.25f, boxLayer);

    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0, 180, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            // Make the player a child of the platform                      // These OnCollision Functions are there to make the active player
            transform.parent = collision.transform;                         // a child of the platform its standing on
        }                                                                   //
    }                                                                       // For some reason if they arent the child of the platform, they dont
                                                                            // follow the platform....
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            // Remove the player from being a child of the platform
            transform.parent = null;
        }
    }

}
