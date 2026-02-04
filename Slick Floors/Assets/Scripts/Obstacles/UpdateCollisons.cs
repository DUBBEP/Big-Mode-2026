using UnityEngine;

public class UpdateCollisons : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D collision)
    {
        // Debug.Log("Exited trigger with: " + collision.gameObject.name);
        // Only re-enable collision for Player and Mop layers
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Mop"))
        {
            // Debug.Log("Re-enabling collision with: " + collision.gameObject.name);
            Collider2D signCollider = GetComponent<Collider2D>();
            if (signCollider != null)
            {
                Physics2D.IgnoreCollision(signCollider, collision, false);
            }
        }
    }
}
