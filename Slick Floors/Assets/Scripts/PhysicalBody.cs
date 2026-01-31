using UnityEngine;

public class PhysicalBody : MonoBehaviour
{
    public float targetHeight = 1.7f; // Desired distance from ground
    public float springStrength = 325f;
    public float damping = 10f;
    public LayerMask groundLayer;
    private Rigidbody2D rb;

    void Start() => rb = GetComponent<Rigidbody2D>();

    void FixedUpdate()
    {
        // Shoot a raycast down to find the floor
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, targetHeight + 2f, groundLayer);

        if (hit.collider != null)
        {
            // how far we are from our target height
            float currentHeight = hit.distance;
            float error = targetHeight - currentHeight;

            // Spring Force
            float upwardForce = (error * springStrength) - (rb.linearVelocity.y * damping);

            // Only push up if we are too low
            if (error > 0)
            {
                rb.AddForce(Vector2.up * upwardForce);
            }
        }
    }
}