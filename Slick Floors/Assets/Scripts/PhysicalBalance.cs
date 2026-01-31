using UnityEngine;

public class PhysicalBalance : MonoBehaviour
{
    public float targetRotation = -90f;
    public float forceStrength = 1f;
    public float damping = 0f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!enabled) return;
        // Difference between current and target angle
        float currentAngle = rb.rotation;
        float angleError = Mathf.DeltaAngle(currentAngle, targetRotation);

        // Apply torque
        float torque = angleError * forceStrength - rb.angularVelocity * damping;
        rb.AddTorque(torque);
    }
}