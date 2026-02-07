using UnityEngine;

public class DamageChecker : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    private System.Collections.Generic.HashSet<GameObject> processedPickups = new System.Collections.Generic.HashSet<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision) => RunChecks(collision.gameObject);

    private void OnCollisionEnter2D(Collision2D collision) => RunChecks(collision.gameObject);

    private void RunChecks(GameObject other)
    {
        if (other.TryGetComponent<IDamageSource>(out IDamageSource sourceObject))
            TakeDamage(sourceObject);
    }

    private void TakeDamage(IDamageSource sourceObject)
    {
        DamageSource src = sourceObject.GetDamageSource();
        src.recievingObject = gameObject;
        playerHealth.TakeDamage(src);
    }
}
