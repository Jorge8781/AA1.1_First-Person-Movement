using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float groundCheckDistance = 1.1f;
    public float jumpForce = 5f;

    public float airControlMultiplier = 0.5f;
    public float backwardMultiplier = 0.5f;
    public float sidewaysMultiplier = 0.75f;
    public float sprintMultiplier = 2f;

    public LayerMask groundLayer;
    private bool isGrounded;
    private bool canJump = true;
    private Jetpack jetpack;

    private Rigidbody rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        jetpack = GetComponent<Jetpack>();

        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        GroundCheck();

        if (animator != null)
        {
            animator.SetBool("IsFloating", !isGrounded);
        }

        HandleMovement();
        HandleJump();
    }

    void GroundCheck()
    {
        isGrounded = Physics.Raycast(
            transform.position + Vector3.up,
            Vector3.down,
            groundCheckDistance,
            groundLayer
        );

        if (isGrounded)
        {
            canJump = true;
        }

        if (jetpack != null)
        {
            jetpack.SetGrounded(isGrounded);
        }
    }

    void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if (animator != null)
        {
            animator.SetFloat("MoveX", x);
            animator.SetFloat("MoveY", z);
        }

        Vector3 move =
            transform.right * x +
            transform.forward * z;

        float speedModifier = 1f;

        if (z < 0)
            speedModifier *= backwardMultiplier;

        if (x != 0 && z == 0)
            speedModifier *= sidewaysMultiplier;

        bool sprinting = Input.GetKey(KeyCode.LeftShift) && z > 0;

        if (sprinting)
            speedModifier *= sprintMultiplier;

        if (!isGrounded)
            speedModifier *= airControlMultiplier;

        Vector3 velocity = move.normalized * moveSpeed * speedModifier;

        velocity.y = rb.linearVelocity.y;

        rb.linearVelocity = velocity;
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && canJump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canJump = false;
        }
    }
}