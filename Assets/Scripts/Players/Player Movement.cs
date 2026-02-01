using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    PlayerInput playerInput;
    Vector2 moveInput;
    [SerializeField] float movementSpeed = 300f;
    [SerializeField] float jumpForce = 10f;
    public bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerInput = GetComponent<PlayerInput>();

        playerInput.actions["Move"].performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInput.actions["Move"].canceled += ctx => moveInput = Vector2.zero;
        playerInput.actions["Jump"].performed += ctx => Jump();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * movementSpeed * Time.fixedDeltaTime, rb.linearVelocityY);
        if (moveInput.x < 0)
            spriteRenderer.flipX = true;
        else if (moveInput.x > 0)
            spriteRenderer.flipX = false;
    }

    private void Jump()
    {
        if (!isGrounded) return;
        AudioManager.Instance.PlaySE(AudioManager.SE_JUMP);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DeathZone"))
        {
            Debug.Log("Player has died!");
            AudioManager.Instance.PlaySE(AudioManager.SE_FAIL);
        }
    }
}
