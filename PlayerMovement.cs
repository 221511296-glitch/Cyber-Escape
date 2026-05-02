using UnityEngine;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    [Header("Movement Bounds")]
    public bool clampToBounds = true;
    public Collider2D movementBoundsCollider;
    public Vector2 minPosition = new Vector2(-9f, -4.5f);
    public Vector2 maxPosition = new Vector2(9f, 4.5f);

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;
    private SpriteRenderer spriteRenderer;
    private bool hasSpeedParam;
    private bool hasIsSideParam;
    private bool hasMoveYParam;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.freezeRotation = true;
        }
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (animator != null)
        {
            hasSpeedParam = animator.parameters.Any(parameter => parameter.name == "speed");
            hasIsSideParam = animator.parameters.Any(parameter => parameter.name == "isSide");
            hasMoveYParam = animator.parameters.Any(parameter => parameter.name == "moveY");
        }
    }

    void Update()
    {
        // Ignore input if the game is paused
        if (Time.timeScale == 0f)
        {
            movement = Vector2.zero;
            if (animator != null && hasSpeedParam)
                animator.SetFloat("speed", 0f);
            return;
        }

        PasswordLevelManager pwdManager = FindObjectOfType<PasswordLevelManager>();
        if (pwdManager != null && (PasswordLevelManager.IsBreachSessionActive || PasswordLevelManager.IsAnyRecoveryPanelOpen))
        {
            movement = Vector2.zero;

            if (animator != null && hasSpeedParam)
                animator.SetFloat("speed", 0f);

            return;
        }

        // Get input
        movement.x = Input.GetAxisRaw("Horizontal"); // -1 left, 0, 1 right
        movement.y = Input.GetAxisRaw("Vertical");   // -1 down, 0, 1 up

        // Move animation speed
        if (animator != null && hasSpeedParam)
            animator.SetFloat("speed", movement.sqrMagnitude);

        // Handle side-facing sprite
        if (movement.x < 0)
        {
            // Left → flip sprite
            spriteRenderer.flipX = true;
            if (animator != null && hasIsSideParam)
                animator.SetBool("isSide", true);
        }
        else if (movement.x > 0)
        {
            // Right → normal sprite
            spriteRenderer.flipX = false;
            if (animator != null && hasIsSideParam)
                animator.SetBool("isSide", true);
        }
        else
        {
            // Not moving horizontally → stop side pose
            if (animator != null && hasIsSideParam)
                animator.SetBool("isSide", false);
        }

        // Optionally handle up/down animations
        if (movement.y != 0 && movement.x == 0)
        {
            if (animator != null && hasIsSideParam)
                animator.SetBool("isSide", false);

            if (animator != null && hasMoveYParam)
                animator.SetFloat("moveY", movement.y);
        }
    }

    void FixedUpdate()
    {
        // Move the player
        Vector2 targetPosition = rb.position + movement * moveSpeed * Time.fixedDeltaTime;

        if (clampToBounds)
        {
            if (movementBoundsCollider != null)
            {
                Bounds bounds = movementBoundsCollider.bounds;
                targetPosition.x = Mathf.Clamp(targetPosition.x, bounds.min.x, bounds.max.x);
                targetPosition.y = Mathf.Clamp(targetPosition.y, bounds.min.y, bounds.max.y);
            }
            else
            {
                targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
                targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);
            }
        }

        rb.MovePosition(targetPosition);
    }

    public void StopMovement()
    {
        movement = Vector2.zero;
        if (rb != null) rb.velocity = Vector2.zero;
        if (animator != null)
        {
            if (hasSpeedParam) animator.SetFloat("speed", 0f);
            if (hasIsSideParam) animator.SetBool("isSide", false);
            if (hasMoveYParam) animator.SetFloat("moveY", 0f);
        }
        this.enabled = false;
    }
}
