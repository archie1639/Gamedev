using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 movement;
    private Vector2 screenBounds;
    private float objectWidth;
    [SerializeField] private Animator animator;
    
    // Reference to jump script to get ground state
    private PlayerJump playerJump;

    private void Start()
    {
        // Get reference to PlayerJump component
        playerJump = GetComponent<PlayerJump>();
        
        // Get screen bounds in world units
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        }
        
        // Get the width of the player object for more accurate boundary checking
        Renderer objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            objectWidth = objectRenderer.bounds.size.x / 2;
        }

        // Auto-assign animator if not set in inspector
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        // Get current keyboard state
        Keyboard keyboard = Keyboard.current;
        
        float input = 0f;
        
        // Check for horizontal input (A/D keys or Arrow keys)
        if (keyboard != null)
        {
            if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
                input = -1f;
            else if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
                input = 1f;
        }
        
        // Calculate and apply movement
        movement.x = input * speed * Time.deltaTime;
        transform.Translate(movement);
        
        // Make character face the direction they're moving
        if (input != 0f)
        {
            // Flip the character's scale on X-axis based on direction
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (input > 0 ? 1 : -1);
            transform.localScale = scale;
        }
        
        // Update animator parameters
        if (animator != null)
        {
            // Set running animation
            animator.SetBool("isRunning", input != 0f);
            
            // Set speed for blend trees (if you're using them)
            animator.SetFloat("Speed", Mathf.Abs(input));
        }
        
        // Clamp position within screen bounds, accounting for object width
        float clampedX = Mathf.Clamp(transform.position.x, -screenBounds.x + objectWidth, screenBounds.x - objectWidth);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
}