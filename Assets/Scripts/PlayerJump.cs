using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Animator animator;

    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private bool isJumpPressed = false;
    private bool isGrounded = false;

    private void Awake()
    {
        inputActions = new PlayerInputActions();

        // Subscribe to input events
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Player.Jump.performed += ctx => isJumpPressed = true;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    void Update()
    {
        // Move left/right
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

        // Handle jump
        if (isJumpPressed && isGrounded)
        {
            Jump();
        }

        isJumpPressed = false; // Reset after use

        // Update animator
        if (animator != null)
        {
            animator.SetBool("isGrounded", isGrounded);
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // Reset Y velocity
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        if (animator != null)
        {
            animator.SetTrigger("Jump");
        }
    }

    // Collision-based ground detection
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
