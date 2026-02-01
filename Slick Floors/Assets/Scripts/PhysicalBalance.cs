using UnityEngine;

public class PhysicalBalance : MonoBehaviour
{
    public float targetRotation = -90f;
    public float forceStrength = 1f;
    public float damping = 0.5f;

    [HideInInspector] public float currentforceStrength;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentforceStrength = forceStrength;
    }

    void FixedUpdate()
    {
        if (!enabled) return;
        // Difference between current and target angle
        float currentAngle = rb.rotation;
        float angleError = Mathf.DeltaAngle(currentAngle, targetRotation);

        // Apply torque
        float torque = (angleError * currentforceStrength) - (rb.angularVelocity * damping);
        rb.AddTorque(torque);
    }
}