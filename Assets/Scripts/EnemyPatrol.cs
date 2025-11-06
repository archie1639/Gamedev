using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    public GameObject pointA;
    public GameObject pointB;

    [Header("Settings")]
    public float speed = 2f;
    
    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public string groundTag = "Ground";

    [Header("Sound Effects")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip monsterSound;
    [SerializeField] private float soundInterval = 3f; // Play sound every 3 seconds

    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;
    private float soundTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB.transform;

        if (anim != null)
            anim.SetBool("isRunning", true);

        // Initialize audio
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        soundTimer = soundInterval;
    }

    void Update()
    {
        // Play monster sound periodically
        soundTimer -= Time.deltaTime;
        
        if (soundTimer <= 0f)
        {
            PlayMonsterSound();
            soundTimer = soundInterval;
        }

        // Check if there's ground ahead
        bool isGroundAhead = CheckForGround();
        
        // If no ground ahead, turn around immediately
        if (!isGroundAhead)
        {
            Flip();
            return;
        }

        // Move towards current point
        if (currentPoint == pointB.transform)
        {
            rb.linearVelocity = new Vector2(speed, 0);
        }
        else
        {
            rb.linearVelocity = new Vector2(-speed, 0);
        }

        // Check if enemy reached a patrol point
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f)
        {
            Flip();
        }
    }
    
    bool CheckForGround()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(groundCheck.position, checkRadius);
        
        foreach (Collider2D collider in hitColliders)
        {
            if (collider.CompareTag(groundTag))
            {
                return true;
            }
        }
        
        return false;
    }
    
    void Flip()
    {
        if (currentPoint == pointB.transform)
        {
            currentPoint = pointA.transform;
            
            // Flip to face left
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else
        {
            currentPoint = pointB.transform;
            
            // Flip to face right
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    private void PlayMonsterSound()
    {
        if (monsterSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(monsterSound);
        }
    }

    // Optional: Play sound when player gets close
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayMonsterSound();
        }
    }
    
    // Visualize the ground check in the editor
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}