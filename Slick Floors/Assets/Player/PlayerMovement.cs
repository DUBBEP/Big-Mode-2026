using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rbody;
    [Header("Movement")]
    public float moveSpeed = 50f;
    public float acceleration = 10f;
    public float deceleration = 2f;
    public float airResist = 0.5f;
    public float velocityPower = 0.9f;
    public float horizontalMovement;
    [Header("Jumping")]
    public float jumpForce = 30f;
    [Header("Ground Check")]
    public Transform groundCheck;
    public Vector2 groundCheckSize = new Vector2(1f, 0.1f);
    public LayerMask groundLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float targetSpeed = horizontalMovement * moveSpeed;
        float speedDif = targetSpeed - rbody.linearVelocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        if (!IsGrounded()){
            accelRate *= airResist;
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
}
