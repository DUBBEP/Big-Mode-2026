using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMovementProfile", menuName = "MovementProfile")]
public class MovementProfileSO : ScriptableObject
{
    [Header("Movement")]
    public float MoveSpeed = 40f;
    public float Acceleration = 20f;
    public float Deceleration = 10f;
    public float AirResist = 0.8f;

    [Header("Jumping")]
    public float JumpForce = 66f;
    public Vector2 WallJumpForce = new Vector2(30f, 50f);
    public float JumpGravityScale = 1;
    public float JumpCancelGravityScale = 2;
    public float FallingGravityScale = 3;

    [Header("Crouch/Fast Fall")]
    [SerializeField] public float FastFallForce = 80f;


    [Header("Feel Improvements")]
    public float JumpBufferTime = 0.15f;
    public float WallJumpMovementLockTime = 0.2f;

    [Header("MopControls")]
    public float MopMoveSpeed = 15f;

    [Header("Sound Effects")]
    public List<AudioClip> jumpSoundFXs;
    public AudioClip landSoundFX;
    public AudioClip walkSoundFX;
    public AudioClip speedySoundFX;

    public List<AudioClip> moppedSoundFXs;
    public float walkSoundTiming = 0.22f;

}
