using UnityEngine;
using UnityEngine.InputSystem;

public class MopBehavior : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    
    [Header("Settings")]
    public float rotationOffset = 0f;
    
    void LateUpdate()
    {
        RotateMopToMouse();
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
}