using UnityEngine;

public class PlayerCollisionsController : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private MopController mop;
    private MovementProfileSO previousMovementProfile;
    private System.Collections.Generic.HashSet<GameObject> processedPickups = new System.Collections.Generic.HashSet<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision) => RunChecks(collision.gameObject);

    private void OnCollisionEnter2D(Collision2D collision) => RunChecks(collision.gameObject);

    private void RunChecks(GameObject other)
    {
        if (other.TryGetComponent<FloorTile>(out FloorTile tileObject))
            UpdateMovementProfile(tileObject.currentMovementProfile);

        if (other.layer == LayerMask.NameToLayer("Pickup"))
        {
            GameObject targetToDestroy = other;
            // Prevent double-processing
            if (targetToDestroy == null || targetToDestroy.activeSelf == false)
                return;
            
            if (processedPickups.Contains(targetToDestroy))
                return;

            processedPickups.Add(targetToDestroy);

            // Deactivate immediately to prevent further collisions
            targetToDestroy.SetActive(false);
            Destroy(targetToDestroy);
            movement.addSign();
        }
    }

    private void UpdateMovementProfile(MovementProfileSO profile)
    {
        if (profile == null)
            Debug.LogError("Missing profile in collisions controller");

        movement.SetMovementProfile(profile);
        mop.moveSpeed = profile.MopMoveSpeed;
        previousMovementProfile = profile;
    }
}
