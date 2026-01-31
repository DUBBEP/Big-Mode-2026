using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;
    private LayerMask groundLayer;

    [Header("Detection Settings")]
    [SerializeField] private Vector2 groundOffset = new Vector2(0f, -1.5f);
    [SerializeField] private Vector2 groundSize = new Vector2(0.95f, 0.1f);
    [SerializeField] private Vector2 wallCheckSize = new Vector2(0.2f, 2.5f); // Height should match player
    [SerializeField] private float wallCheckOffset = 0.5f; // Horizontal distance from center

    [Header("Movement")]
    public float moveSpeed = 40f;
    public float acceleration = 20f;
    public float airResist = 0.8f;

    [Header("Jumping")]
    public float jumpForce = 66f;
    public Vector2 wallJumpForce = new Vector2(30f, 50f);

    [Header("Feel Improvements")]
    [SerializeField] private float coyoteTime = 0.15f;
    [SerializeField] private float jumpBufferTime = 0.15f;
    [SerializeField] private float wallJumpMovementLockTime = 0.2f;

    private float _coyoteCounter;
    private float _jumpBufferCounter;
    private float _wallJumpLockCounter;

    public bool isGrounded;
    public int wallSide;
    private bool _canDoubleJump;
    private float _horizontalInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.mass = 3f;
        rb.gravityScale = 5f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.freezeRotation = true;
        groundLayer = LayerMask.GetMask("Ground");
    }

    void Update()
    {
        CheckPhysics();
        HandleTimers();
    }

    void FixedUpdate() => ApplyMovement();

    private void CheckPhysics()
    {
        isGrounded = Physics2D.OverlapBox((Vector2)transform.position + groundOffset, groundSize, 0f, groundLayer);

        if (isGrounded)
        {
            _coyoteCounter = coyoteTime;
            _canDoubleJump = true;
        }

        // BoxCast for Left and Right sides
        Vector2 leftBoxPos = (Vector2)transform.position + new Vector2(-wallCheckOffset, 0);
        Vector2 rightBoxPos = (Vector2)transform.position + new Vector2(wallCheckOffset, 0);

        bool leftWall = Physics2D.OverlapBox(leftBoxPos, wallCheckSize, 0f, groundLayer);
        bool rightWall = Physics2D.OverlapBox(rightBoxPos, wallCheckSize, 0f, groundLayer);

        if (rightWall) wallSide = 1;
        else if (leftWall) wallSide = -1;
        else wallSide = 0;
    }

    private void HandleTimers()
    {
        _coyoteCounter -= Time.deltaTime;
        _jumpBufferCounter -= Time.deltaTime;
        _wallJumpLockCounter -= Time.deltaTime;

        if (_jumpBufferCounter > 0)
        {
            if (_coyoteCounter > 0)
            {
                ExecuteJump(Vector2.up * jumpForce);
            }
            else if (wallSide != 0 && !isGrounded)
            {
                _wallJumpLockCounter = wallJumpMovementLockTime;
                ExecuteJump(new Vector2(wallJumpForce.x * -wallSide, wallJumpForce.y));
            }
            else if (_canDoubleJump)
            {
                ExecuteJump(Vector2.up * jumpForce);
                _canDoubleJump = false;
            }
        }
    }

    private void ExecuteJump(Vector2 force)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x * 0.5f, 0);
        rb.AddForce(force, ForceMode2D.Impulse);
        _jumpBufferCounter = 0;
        _coyoteCounter = 0;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed) _jumpBufferCounter = jumpBufferTime;

        if (context.canceled && rb.linearVelocity.y > 0)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
    }

    public void OnMove(InputAction.CallbackContext context) => _horizontalInput = context.ReadValue<Vector2>().x;

    private void ApplyMovement()
    {
        if (_wallJumpLockCounter > 0) return;

        float targetSpeed = _horizontalInput * moveSpeed;
        float speedDif = targetSpeed - rb.linearVelocity.x;
        float accelRate = isGrounded ? acceleration : acceleration * airResist;
        rb.AddForce(Vector2.right * speedDif * accelRate);
    }

    private void OnDrawGizmosSelected()
    {
        // Ground
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((Vector2)transform.position + groundOffset, groundSize);

        // Walls
        Gizmos.color = Color.blue;
        Vector2 leftBoxPos = (Vector2)transform.position + new Vector2(-wallCheckOffset, 0);
        Vector2 rightBoxPos = (Vector2)transform.position + new Vector2(wallCheckOffset, 0);
        Gizmos.DrawWireCube(leftBoxPos, wallCheckSize);
        Gizmos.DrawWireCube(rightBoxPos, wallCheckSize);
    }
}