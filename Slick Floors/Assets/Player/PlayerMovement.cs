using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;
    private LayerMask groundLayer;

    [SerializeField] private MovementProfileSO movementData;

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
        isGrounded = Physics2D.OverlapBox(
            (Vector2)transform.position + movementData.groundOffset, 
            movementData.groundSize, 0f, groundLayer);

        if (isGrounded)
        {
            _coyoteCounter = movementData.coyoteTime;
            if (movementData.doubleJumpEnabled)
                _canDoubleJump = true;
            else
                _canDoubleJump = false;
        }
        else if (rb.linearVelocity.y < 0f)
        {
            rb.gravityScale = movementData.fallingGravityScale;
        } 

        // BoxCast for Left and Right sides
        Vector2 leftBoxPos = (Vector2)transform.position + new Vector2(-movementData.wallCheckOffset, 0);
        Vector2 rightBoxPos = (Vector2)transform.position + new Vector2(movementData.wallCheckOffset, 0);

        bool leftWall = Physics2D.OverlapBox(leftBoxPos, movementData.wallCheckSize, 0f, groundLayer);
        bool rightWall = Physics2D.OverlapBox(rightBoxPos, movementData.wallCheckSize, 0f, groundLayer);

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
            if (wallSide != 0 && !isGrounded)
            {
                _wallJumpLockCounter = movementData.wallJumpMovementLockTime;
                ExecuteJump(new Vector2(movementData.wallJumpForce.x * -wallSide, movementData.wallJumpForce.y));
                Debug.Log("Did wall jump");
            }
            else if (_coyoteCounter > 0)
            {
                ExecuteJump(Vector2.up * movementData.jumpForce);
                Debug.Log("Did normal jump");
            }
            else if (_canDoubleJump)
            {
                ExecuteJump(Vector2.up * movementData.jumpForce);
                _canDoubleJump = false;
                Debug.Log("Did double jump");
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
        if (context.performed)
        {
            _jumpBufferCounter = movementData.jumpBufferTime;
            rb.gravityScale = movementData.jumpGravityScale;
        }

        if (context.canceled && rb.linearVelocity.y > 0)
            rb.gravityScale = movementData.jumpCancelGravityScale;
    }

    public void OnMove(InputAction.CallbackContext context) => _horizontalInput = context.ReadValue<Vector2>().x;

    private void ApplyMovement()
    {
        if (_wallJumpLockCounter > 0) return;

        float targetSpeed = _horizontalInput * movementData.moveSpeed;
        float speedDif = targetSpeed - rb.linearVelocity.x;
        float accelType = (Mathf.Abs(targetSpeed) > 0.01f) ? movementData.acceleration : movementData.deceleration;
        float accelRate = isGrounded ? accelType : accelType * movementData.airResist;
        rb.AddForce(Vector2.right * speedDif * accelRate);
    }

    private void OnDrawGizmosSelected()
    {
        // Ground
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((Vector2)transform.position + movementData.groundOffset, movementData.groundSize);

        // Walls
        Gizmos.color = Color.blue;
        Vector2 leftBoxPos = (Vector2)transform.position + new Vector2(-movementData.wallCheckOffset, 0);
        Vector2 rightBoxPos = (Vector2)transform.position + new Vector2(movementData.wallCheckOffset, 0);
        Gizmos.DrawWireCube(leftBoxPos, movementData.wallCheckSize);
        Gizmos.DrawWireCube(rightBoxPos, movementData.wallCheckSize);
    }
}