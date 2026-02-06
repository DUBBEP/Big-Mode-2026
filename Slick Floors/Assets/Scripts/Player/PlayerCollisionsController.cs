using UnityEngine;

public class PlayerCollisionsController : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;
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
            GameObject rootObject = other.transform.root.gameObject;
            // Prevent double-processing
            if (rootObject == null || rootObject.activeSelf == false)
                return;
            int instanceId = rootObject.GetInstanceID();
            if (processedPickups.Contains(rootObject))
                return;

            processedPickups.Add(rootObject);

            // Deactivate immediately to prevent further collisions
            rootObject.SetActive(false);
            Destroy(rootObject);
            movement.addSign();
        }
    }

    private void UpdateMovementProfile(MovementProfileSO profile)
    {
        if (profile == null)
            Debug.LogError("Missing profile in collisions controller");

        movement.SetMovementProfile(profile);

        if (previousMovementProfile != profile && profile.splashSoundFX != null)
        {
            SoundFXManager.Instance.playSoundFXClip(profile.splashSoundFX, this.transform);
        }
        previousMovementProfile = profile;
    }
}
