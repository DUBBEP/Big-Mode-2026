using UnityEngine;

public class FixedObject : MonoBehaviour
{
    public Transform anchor;
    public float strength;
    public float distanceFromAnchor = 0f;

    void Update()
    {
        float distanceError = Vector2.Distance(transform.position, anchor.position) - distanceFromAnchor;
        Vector2 directionToAnchor = (anchor.position - transform.position).normalized;
        Vector2 correctionForce = directionToAnchor * distanceError * strength;
        // handBody cannot be a rigidbody, so we move it directly
        transform.position += (Vector3)(correctionForce * Time.deltaTime);
    }
}
