using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator animator;
    private Rigidbody2D rb;
    private PlayerMovement playerMovement;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // Check movement state
        bool isMoving = Mathf.Abs(rb.linearVelocityX) > 0.1f;
        bool grounded = playerMovement.isGrounded;

        // Set animation states
        animator.SetBool("isJumping", !grounded);
        animator.SetBool("isWalking", grounded && isMoving);
        animator.SetBool("isIdle", grounded && !isMoving);
        animator.SetBool("isFalling", !grounded && rb.linearVelocity.y < 0);
    }
}
