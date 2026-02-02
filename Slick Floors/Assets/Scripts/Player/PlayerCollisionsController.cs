using UnityEngine;

public class PlayerCollisionsController : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private PlayerMovement movement;

    private void OnTriggerEnter2D(Collider2D collision) => RunChecks(collision.gameObject);

    private void OnCollisionEnter2D(Collision2D collision) => RunChecks(collision.gameObject);

    private void RunChecks(GameObject other)
    {
        if (other.TryGetComponent<IDamageSource>(out IDamageSource sourceObject))
            TakeDamage(sourceObject);
        else if (other.TryGetComponent<FloorTile>(out FloorTile tileObject))
            UpdateMovementProfile(tileObject.currentMovementProfile);
    }

    private void TakeDamage(IDamageSource sourceObject)
    {
        DamageSource src = sourceObject.GetDamageSource();
        src.recievingObject = gameObject;
        playerHealth.TakeDamage(src);
    }

    private void UpdateMovementProfile(MovementProfileSO profile)
    {
        if (profile == null)
            Debug.LogError("Missing profile in collisions controller");
        
        movement.SetMovementProfile(profile);
    }
}
