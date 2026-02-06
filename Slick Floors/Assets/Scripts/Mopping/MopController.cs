using UnityEngine;
using UnityEngine.InputSystem;

public class MopController : MonoBehaviour
{
    [Header("References")]
    public Rigidbody2D rb;
    public Transform mopTip;

    [Header("Settings")]
    public float deadZoneRadius = 0.5f;
    public float rotationSmoothTime = 0.1f;
    public float moveSpeed = 15f;

    private float currentAngleVelocity;

    void FixedUpdate()
    {
        if (Camera.main == null || Mouse.current == null || rb == null || mopTip == null) return;

        // Mouse coords
        Vector3 mouseInput = Mouse.current.position.ReadValue();
        float distanceToCamera = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseInput.x, mouseInput.y, distanceToCamera));

        MoveMopTip(mouseWorldPos);
        RotateTowardsMouse(mouseWorldPos);
    }

    void MoveMopTip(Vector3 mouseWorldPos)
    {
        Vector2 direction = (Vector2)mouseWorldPos - (Vector2)mopTip.position;
        rb.AddForce(direction * moveSpeed);
    }

    void RotateTowardsMouse(Vector3 mouseWorldPos)
    {
        // Direction to mouse from rb
        Vector2 directionToMouse = (Vector2)mouseWorldPos - rb.position;
        float distToMouse = directionToMouse.magnitude;

        // Rotation and smoothing
        float currentAngle = rb.rotation;
        float targetAngle = currentAngle;

        if (distToMouse > deadZoneRadius)
        {
            targetAngle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
        }

        float smoothedAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref currentAngleVelocity, rotationSmoothTime);

        rb.MoveRotation(smoothedAngle);
    }
}