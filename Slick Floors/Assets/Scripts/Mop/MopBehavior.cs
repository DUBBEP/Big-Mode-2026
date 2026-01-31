using UnityEngine;
using UnityEngine.InputSystem;

public class MopBehavior : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    
    [Header("Settings")]
    public Transform mopTip; 
    public float mopRange = 0.5f; 
    public float rotationOffset = -90f;
    public float minDistanceBetweenPuddles = 0.5f;

    [Header("Surface Materials")]
    public LayerMask groundLayer;
    public GameObject cleanSurface;

    private Vector2 lastPuddlePos = new Vector2(999f, 999f);

    void OnStart()
    {
    }

    void LateUpdate()
    {
        RotateMopToMouse();
        if (Mouse.current.leftButton.isPressed) 
        {
            ApplyMopEffect();
        }
    }

    void RotateMopToMouse()
    {
        if (Camera.main == null) return;
        Vector3 mouseInput = Mouse.current.position.ReadValue();
        float distanceToCamera = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(mouseInput.x, mouseInput.y, distanceToCamera));
        
        Vector2 direction = (Vector2)mousePos - (Vector2)transform.position;
        if (direction.sqrMagnitude > 0.1f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle + rotationOffset);
        }
    }

    void ApplyMopEffect()
    {
        // Raycast down from the mop tip
        RaycastHit2D hit = Physics2D.Raycast(mopTip.position, Vector2.down, 1f, groundLayer);
        if (hit.collider != null)
        {
            // Only spawn if we've moved far enough from the last puddle
            if (Vector2.Distance(lastPuddlePos, hit.point) > minDistanceBetweenPuddles)
            {
                Vector3 spawnPos = new Vector3(hit.point.x, hit.point.y + 0.05f, -0.1f);
                Instantiate(cleanSurface, spawnPos, Quaternion.identity);
    
                lastPuddlePos = hit.point;
                Debug.Log("Clean surface placed!");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (mopTip != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(mopTip.position, mopRange);
        }
    }
}