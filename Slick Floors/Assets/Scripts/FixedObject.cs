using UnityEngine;

public class FixedObject : MonoBehaviour
{
    public Transform anchor;
    public Transform fixedObject;
    public float strength;
    public float distanceFromAnchor = 0f;

    void Update()
    {
        float distanceError = Vector2.Distance(fixedObject.position, anchor.position) - distanceFromAnchor;
        Vector2 directionToAnchor = (anchor.position - fixedObject.position).normalized;
        Vector2 correctionForce = directionToAnchor * distanceError * strength;
        // handBody cannot be a rigidbody, so we move it directly
        fixedObject.position += (Vector3)(correctionForce * Time.deltaTime);
    }
}
