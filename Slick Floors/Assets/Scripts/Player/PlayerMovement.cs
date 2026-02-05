using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GenericEventSO onPlayerDeath;
    [SerializeField] private MovementProfileSO movementData;

    [Header("Detection Settings")]
    [SerializeField] private Vector2 groundOffset = new Vector2();
    [SerializeField] private Vector2 groundSize = new Vector2();
    [SerializeField] private Vector2 wallCheckSize = new Vector2(); // Height should match player
    [SerializeField] private Vector2 wallCheckOffset = new Vector2(); // Horizontal distance from center
    [SerializeField] private float groundCheckRadius;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform heldItem;
    [SerializeField] private PhysicalBody physicalBody;
    [SerializeField] private Transform leftFootGroundedCheck;
    [SerializeField] private Transform rightFootGroundedCheck;

    private LayerMask groundLayer;

    private LayerMask wallLayer;

    [Header("Walk Animation")]
    [SerializeField] private bool useWalk;
    [SerializeField] private float walkStepSpeed;
    [SerializeField] private float stepSize;
    [SerializeField] private float legForce;
    [SerializeField] private PhysicalBalance leftLegBone;
    [SerializeField] private PhysicalBalance rightLegBone;


    private float _jumpBufferCounter;
    private float _wallJumpLockCounter;

    private bool _isGrounded;
    private int _wallSide;
    private float _horizontalInput;
    private bool _isCrouching = false;

    private CautionPlace _cautionPlace;

    void Awake()
    {
        rb.mass = 3f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        groundLayer = LayerMask.GetMask("Ground");
        wallLayer = LayerMask.GetMask("Wall");

        if (heldItem != null)
            _cautionPlace = heldItem.GetComponent<CautionPlace>();
    }

    void Update()
    {
        CheckPhysics();
        HandleTimers();

        // Pass input to the CautionPlace component
        if (_cautionPlace != null)
            _cautionPlace.HandleInput(Mouse.current.rightButton.isPressed);

        if (useWalk)
        {
            if (Mathf.Abs(_horizontalInput) > 0f)
                AnimateLegs(_horizontalInput);
            else
                ResetLegs();
        }
    }

    void FixedUpdate() => ApplyMovement();

    private void CheckPhysics()
    {
        _isGrounded = Physics2D.OverlapCircle(leftFootGroundedCheck.position, groundCheckRadius, groundLayer) ||
                      Physics2D.OverlapCircle(leftFootGroundedCheck.position, groundCheckRadius, groundLayer);

        if (!_isGrounded && rb.linearVelocity.y < 0f)
            rb.gravityScale = movementData.fallingGravityScale;

        // BoxCast for Left and Right sides
        Vector2 leftBoxPos = (Vector2)transform.position + new Vector2(-wallCheckOffset.x, wallCheckOffset.y);
        Vector2 rightBoxPos = (Vector2)transform.position + new Vector2(wallCheckOffset.x, wallCheckOffset.y);

        bool leftWall = Physics2D.OverlapBox(leftBoxPos, wallCheckSize, 0f, wallLayer);
        bool rightWall = Physics2D.OverlapBox(rightBoxPos, wallCheckSize, 0f, wallLayer);

        if (rightWall) _wallSide = 1;
        else if (leftWall) _wallSide = -1;
        else _wallSide = 0;
    }

    private void HandleTimers()
    {
        _jumpBufferCounter -= Time.deltaTime;
        _wallJumpLockCounter -= Time.deltaTime;

        if (_jumpBufferCounter > 0)
        {
            if (_isGrounded)
            {
                ExecuteJump(Vector2.up * movementData.jumpForce);
                Debug.Log("Did grounded jump");
            }
            else if (_wallSide !=  0)
            {
                ExecuteJump(new Vector2(movementData.wallJumpForce.x * -_wallSide, movementData.wallJumpForce.y));
                Debug.Log("Did wall jump");
            }
        }
    }

    private void ExecuteJump(Vector2 force)
    {
        rb.gravityScale = movementData.jumpGravityScale;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        rb.AddForce(force, ForceMode2D.Impulse);
        _jumpBufferCounter = 0;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            _jumpBufferCounter = movementData.jumpBufferTime;

        if (context.canceled && rb.linearVelocity.y > 0)
            rb.gravityScale = movementData.jumpCancelGravityScale;
    }

    public void OnMove(InputAction.CallbackContext context) =>
        _horizontalInput = context.ReadValue<Vector2>().x;

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed && !_isCrouching)
                StartCrouch();
        else if (context.canceled && _isCrouching)
                StopCrouch();
    }
    private void ApplyMovement()
    {
        if (_wallJumpLockCounter > 0) return;

        float targetSpeed = _horizontalInput * movementData.moveSpeed;
        float speedDif = targetSpeed - rb.linearVelocity.x;
        float accelType = (Mathf.Abs(targetSpeed) > 0.01f) ? movementData.acceleration : movementData.deceleration;
        float accelRate = _isGrounded ? accelType : accelType * movementData.airResist;
        rb.AddForce(Vector2.right * speedDif * accelRate);
    }

    private void OnDrawGizmosSelected()
    {
        // Walls
        Gizmos.color = Color.blue;
        Vector2 leftBoxPos = (Vector2)transform.position + new Vector2(-wallCheckOffset.x, wallCheckOffset.y);
        Vector2 rightBoxPos = (Vector2)transform.position + new Vector2(wallCheckOffset.x, wallCheckOffset.y);
        Gizmos.DrawWireCube(leftBoxPos, wallCheckSize);
        Gizmos.DrawWireCube(rightBoxPos, wallCheckSize);

        // ground
        Gizmos.DrawWireSphere(leftFootGroundedCheck.position, groundCheckRadius);
        Gizmos.DrawWireSphere(rightFootGroundedCheck.position, groundCheckRadius);
    }

    public void AnimateLegs(float dir)
    {
        // Create a cycle based on time (Sine Wave)
        float timer = Time.time * walkStepSpeed;
        leftLegBone.currentforceStrength = legForce;
        rightLegBone.currentforceStrength = legForce;


        // Calculate leg angles
        // We add PI to the right leg so it moves opposite to the left leg
        // Added -90f offset so legs point down instead of right (0 degrees)
        float leftTarget = -90f + (Mathf.Sin(timer) * stepSize * dir);
        float rightTarget = -90f + (Mathf.Sin(timer + Mathf.PI) * stepSize * dir);

        // Apply to your PhysicalBalance scripts
        if (leftLegBone) leftLegBone.targetRotation = leftTarget;
        if (rightLegBone) rightLegBone.targetRotation = rightTarget;
    }

    public void ResetLegs()
    {
        // Return legs to neutral (-90 degrees) nicely
        if (leftLegBone)
        {
            leftLegBone.targetRotation = Mathf.Lerp(leftLegBone.targetRotation, -90, 0.1f);
            leftLegBone.currentforceStrength = leftLegBone.forceStrength;
        }
        if (rightLegBone)
        {
            rightLegBone.targetRotation = Mathf.Lerp(rightLegBone.targetRotation, -90, 0.1f);
            rightLegBone.currentforceStrength = rightLegBone.forceStrength;
        }
    }

    public void addSign()
    {
        Debug.Log("Added a sign");
        if (_cautionPlace != null) _cautionPlace.AddSign();
    }

    private void StartCrouch()
    {
        _isCrouching = true;
        physicalBody.enabled = false;

        rb.linearVelocity = new Vector2 (rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.down * movementData.fastFallForce, ForceMode2D.Impulse);
    }

    private void StopCrouch()
    {
        _isCrouching = false;
        physicalBody.enabled = true;
    }
    public void SetMovementProfile(MovementProfileSO data) => movementData = data;

    private void OnDeath(GameEventPayload payload) => enabled = false;

    private void OnEnable() => onPlayerDeath.RegisterListener(OnDeath);

    private void OnDisable() => onPlayerDeath.UnregisterListener(OnDeath);
}