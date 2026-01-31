using UnityEngine;

[CreateAssetMenu(fileName = "NewMovementProfile", menuName = "MovementProfile")]
public class MovementProfileSO : ScriptableObject
{
        [Header("Detection Settings")]
    public Vector2 groundOffset = new Vector2(0f, -1.5f);
    public Vector2 groundSize = new Vector2(0.95f, 0.1f);
    public Vector2 wallCheckSize = new Vector2(0.2f, 2.5f); // Height should match player
    public float wallCheckOffset = 0.5f; // Horizontal distance from center

    [Header("Movement")]
    public float moveSpeed = 40f;
    public float acceleration = 20f;
    public float deceleration = 10f;
    public float airResist = 0.8f;
    public bool doubleJumpEnabled;

    [Header("Jumping")]
    public float jumpForce = 66f;
    public Vector2 wallJumpForce = new Vector2(30f, 50f);
    public float jumpGravityScale = 1;
    public float jumpCancelGravityScale = 2;
    public float fallingGravityScale = 3;

    [Header("Feel Improvements")]
    public float coyoteTime = 0.15f;
    public float jumpBufferTime = 0.15f;
    public float wallJumpMovementLockTime = 0.2f;
}
