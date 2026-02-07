using UnityEngine;

public class OneWayLeash : MonoBehaviour
{
    public Transform root;         // Drag your shoulder/hip here
    public Vector2 anchorOffset;   // Where on the root the leash attaches (local space)
    public float maxDistance = 2f; // Your limb length
    private Rigidbody2D rb;

    void Start() => rb = GetComponent<Rigidbody2D>();

    void Update()
    {
        if (root == null) return;

        // Calculate world position of the anchor point respecting root's position, rotation, and scale
        Vector2 effectiveRootPos = root.TransformPoint(anchorOffset);

        Vector2 offset = (Vector2)transform.position - effectiveRootPos;
        float currentDistance = offset.magnitude;

        // If we've gone past the leash length...
        if (currentDistance > maxDistance)
        {
            // Calculate the position exactly at the edge of the radius
            Vector2 clampedPosition = effectiveRootPos + (offset.normalized * maxDistance);

            // Move the target back to the edge without affecting the root's physics
            rb.MovePosition(clampedPosition);

        }
    }

    private void OnDrawGizmosSelected()
    {
        if (root == null) return;

        // Calculate world position of the anchor
        Vector2 effectiveRootPos = root.TransformPoint(anchorOffset);

        // Draw Root Center
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(root.position, 0.1f);

        // Draw Anchor Point
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(effectiveRootPos, 0.1f);
        Gizmos.DrawLine(root.position, effectiveRootPos);

        // Draw Max Distance Circle
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(effectiveRootPos, maxDistance);

        // Draw Line to Target
        Gizmos.color = Color.white;
        Gizmos.DrawLine(effectiveRootPos, transform.position);
    }
}