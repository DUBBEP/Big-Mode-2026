using UnityEngine;
using UnityEngine.InputSystem;

public class WalkingAnimation : MonoBehaviour
{
    [SerializeField]private float walkStepSpeed;
    [SerializeField]private float stepSize;
    [SerializeField]private PhysicalBalance leftLegBone;
    [SerializeField]private PhysicalBalance rightLegBone;

    private PlayerMovement _playerMovement;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void FixedUpdate()
    {
        float dir = _playerMovement.HorizontalInput;

        if (Mathf.Abs(dir) > 0)
            AnimateLegs(dir);
        else
            ResetLegs();
    }

    public void AnimateLegs(float dir)
    {
        Debug.Log("Running animate legs");
        // Create a cycle based on time (Sine Wave)
        float timer = Time.time * walkStepSpeed;

        // Calculate leg angles
        // We add PI to the right leg so it moves opposite to the left leg
        // Added -90f offset so legs point down instead of right (0 degrees)
        float leftTarget = -90f + (Mathf.Sin(timer) * stepSize * dir);
        float rightTarget = -90f + (Mathf.Sin(timer + Mathf.PI) * stepSize * dir);

        // Apply to your PhysicalBalance scripts
        if (leftLegBone) leftLegBone.targetRotation = leftTarget;
        if (rightLegBone) rightLegBone.targetRotation = rightTarget;
    }

    public void ResetLegs()
    {
        Debug.Log("Running Reset legs");
        // Return legs to neutral (-90 degrees) nicely
        if (leftLegBone) leftLegBone.targetRotation = Mathf.Lerp(leftLegBone.targetRotation, -90, 0.1f);
        if (rightLegBone) rightLegBone.targetRotation = Mathf.Lerp(rightLegBone.targetRotation, -90, 0.1f);
    }
}