using UnityEngine;

public class PlayerHurtBoxController : MonoBehaviour
{
    [SerializeField] private Health playerHealth;

    private void OnTriggerEnter2D(Collider2D collision) =>
        TryTakeDamage(collision.gameObject);

    private void OnCollisionEnter2D(Collision2D collision) =>
        TryTakeDamage(collision.gameObject);

    public void TryTakeDamage(GameObject other)
    {
        if (other.TryGetComponent<IDamageSource>(out IDamageSource sourceObject))
        {
            DamageSource src = sourceObject.GetDamageSource();
            src.recievingObject = gameObject;
            playerHealth.TakeDamage(src);
        }
    }
}
