using UnityEngine;
using UnityEngine.InputSystem;

public class MopController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Rigidbody2D mopBody;
    public Transform mopTip;
    public float deadZoneRadius = 0.5f;
    public float rotationSmoothTime = 0.1f;
    public float mopForce = 10f;

    private float currentAngleVelocity;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        RotateTowardsMouse();
        // click to mop
        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            Mop();
        }
    }

    void RotateTowardsMouse()
    {
        if (Camera.main == null || Mouse.current == null || mopBody == null) return;

        // Mouse coords
        Vector3 mouseInput = Mouse.current.position.ReadValue();
        float distanceToCamera = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseInput.x, mouseInput.y, distanceToCamera));

        // Direction to mouse
        Vector2 directionToMouse = (Vector2)mouseWorldPos - mopBody.position;
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
    void Mop()
    {
        // apply a force at the mop tip position in the direction the mop is facing
        if (mopTip == null || rb == null) return;
        Vector2 mopDirection = (Vector2)mopTip.position - (Vector2)mopBody.position;
        mopDirection.Normalize();
        rb.AddForceAtPosition(mopDirection * mopForce, mopTip.position, ForceMode2D.Impulse);
    }
}


