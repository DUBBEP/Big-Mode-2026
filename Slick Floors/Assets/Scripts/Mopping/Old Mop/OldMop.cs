using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class OldMop : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    [Header("Settings")]
    public Transform mopTip;
    public float mopRange = 0.5f;
    public float rotationOffset = -90f;
    public float minDistanceBetweenPuddles = 0.5f;
    public PlayerMovement player;

    [Header("Surface Materials")]
    public LayerMask groundLayer;
    private Collider2D mopCollider;
    private void Start()
    {
        mopCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        RotateMopToMouse();
        if (Mouse.current.leftButton.isPressed)
        {
            mopCollider.enabled = true;
        }
        else
        {
            mopCollider.enabled = false;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Mop has entered trigger");
        if (collision.TryGetComponent<FloorTile>(out FloorTile tile))
        {
            Debug.Log($"Mop has found tile");
            tile.ChangeTile(GroundType.Clean);
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