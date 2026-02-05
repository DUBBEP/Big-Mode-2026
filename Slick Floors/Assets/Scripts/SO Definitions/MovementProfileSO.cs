using UnityEngine;

[CreateAssetMenu(fileName = "NewMovementProfile", menuName = "MovementProfile")]
public class MovementProfileSO : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 40f;
    public float acceleration = 20f;
    public float deceleration = 10f;
    public float airResist = 0.8f;

    [Header("Jumping")]
    public float jumpForce = 66f;
    public Vector2 wallJumpForce = new Vector2(30f, 50f);
    public float jumpGravityScale = 1;
    public float jumpCancelGravityScale = 2;
    public float fallingGravityScale = 3;

    [Header("Crouch/Fast Fall")]
    [SerializeField] public float fastFallForce = 80f;


    [Header("Feel Improvements")]
    public float jumpBufferTime = 0.15f;
    public float wallJumpMovementLockTime = 0.2f;
}
