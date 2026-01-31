using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rbody;
    [Header("Movement")]
    public float moveSpeed = 40f;
    public float acceleration = 20f;
    public float deceleration = 8f;
    public float airResist = 0.5f;
    public float velocityPower = 0.9f;
    public float horizontalMovement;
    [Header("Jumping")]
    public float jumpForce = 30f;
    [Header("Ground Check")]
    public Transform groundCheck;
    public Vector2 groundCheckSize = new Vector2(1f, 0.1f);
    public LayerMask groundLayer;
    [Header("On Surface Materials")]
    bool onCleanSurface = false;
    bool onDirtySurface = false;
    bool onSuperSlickSurface = false;
    [Header("Surface Materials Acceleration Multipliers")]
    public float cleanSurfaceMultiplier = 2f;
    public float dirtySurfaceMultiplier = 0.4f;
    public float superSlickSurfaceMultiplier = 5f;
    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        groundCheck = transform.Find("GroundCheck");
        groundLayer= LayerMask.GetMask("Ground");
    }
    
    void Start()
    {
    }

    void FixedUpdate()
    {
        float targetSpeed = horizontalMovement * moveSpeed;
        float speedDif = targetSpeed - rbody.linearVelocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        if (!IsGrounded()){
            accelRate *= airResist;
        }
        if (onSuperSlickSurface){
            accelRate *= superSlickSurfaceMultiplier;
        }
        else if (onCleanSurface){
            accelRate *= cleanSurfaceMultiplier;
        }
        else if (onDirtySurface){
            accelRate *= dirtySurfaceMultiplier;
        }
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velocityPower) * Mathf.Sign(speedDif);
        rbody.AddForce(movement * Vector2.right);
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context){
        if (context.performed && IsGrounded())
        {
            rbody.linearVelocity = new Vector2(rbody.linearVelocity.x, jumpForce);
        }
        if (context.canceled && rbody.linearVelocity.y > 0)
        {
            rbody.linearVelocity = new Vector2(rbody.linearVelocity.x, rbody.linearVelocity.y * 0.5f);
        }
    }
    private bool IsGrounded()
    {
        if (Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer))
        {
            return true;
        }
        return false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CleanSurface"))
        {
            onCleanSurface = true;
        }
        if (other.CompareTag("DirtySurface"))
        {
            onDirtySurface = true;
        }
        if (other.CompareTag("SuperSlickSurface"))
        {
            onSuperSlickSurface = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("CleanSurface"))
        {
            onCleanSurface = false;
        }
        if (other.CompareTag("DirtySurface"))
        {
            onDirtySurface = false;
        }
        if (other.CompareTag("SuperSlickSurface"))
        {
            onSuperSlickSurface = false;
        }
    }
}