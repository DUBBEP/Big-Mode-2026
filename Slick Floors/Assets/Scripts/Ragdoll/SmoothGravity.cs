using UnityEngine;

public class SmoothGravity : MonoBehaviour
{
    [Header("Gravity Alignment")]
    public float rotationSpeed = 3f;
    public float rotationOffset = 0f;

    void Update()
    {

        Vector2 gravity = Physics2D.gravity;

        // Only rotate if gravity exists
        if (gravity.sqrMagnitude > 0.001f)
        {
            // Calculate the angle of the gravity vector
            float angle = Mathf.Atan2(gravity.y, gravity.x) * Mathf.Rad2Deg;

            // Apply user defined offset
            angle += rotationOffset;

            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

            // Smoothly rotate the bone around its pivot
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
