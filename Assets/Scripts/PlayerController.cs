using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private Animator animator;

    [Header("Death Settings")]
    [SerializeField] private float respawnDelay = 2f;

    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private bool isJumpPressed = false;
    private bool isGrounded = false;
    private bool isDead = false;

    private void Awake()
    {
        inputActions = new PlayerInputActions();

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

    private void Update()
    {
        // Don't allow movement if dead
        if (isDead) return;

        // Handle animations
        if (animator != null)
        {
            animator.SetBool("isGrounded", isGrounded);
            animator.SetBool("isRunning", Mathf.Abs(moveInput.x) > 0.1f);
            animator.SetFloat("Speed", Mathf.Abs(moveInput.x));
        }

        // Flip character facing direction
        if (moveInput.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(moveInput.x);
            transform.localScale = scale;
        }

        // Jump
        if (isJumpPressed && isGrounded)
        {
            Jump();
        }

        isJumpPressed = false; // Reset jump flag
    }

    private void FixedUpdate()
    {
        // Don't allow movement if dead
        if (isDead) return;

        // Apply horizontal movement
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check for ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        // Check for enemy collision
        if (collision.gameObject.CompareTag("Enemy") && !isDead)
        {
            Die();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void Die()
    {
        isDead = true;

        // Stop all movement
        rb.linearVelocity = Vector2.zero;
        moveInput = Vector2.zero;

        // Trigger death animation
        if (animator != null)
        {
            animator.SetTrigger("Death");
        }

        // Optional: Disable collider so player doesn't interact with objects
        GetComponent<Collider2D>().enabled = false;

        // Show Game Over UI after death animation finishes
        Invoke("ShowGameOverScreen", respawnDelay);

        Debug.Log("Player died!");
    }

    private void ShowGameOverScreen()
    {
        // Call GameManager to show Game Over UI
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ShowGameOver();
        }
    }
}