using UnityEngine;

public class MopHandle : MonoBehaviour
{
    public FixedObject RightHandSolver_Target;
    public FixedObject LeftHandSolver_Target;
    public Transform mopAngle;
    private bool handsInversed = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        // euler angle 270 -> 90 is default, rest needs inversed hands
        float mopRotationZ = mopAngle.eulerAngles.z;
        // Debug.Log("Mop Z Rotation: " + mopRotationZ);
        if (mopRotationZ > 90f && mopRotationZ < 270f && !handsInversed)
        {
            // Inverse hands
            Transform tempAnchor = RightHandSolver_Target.anchor;
            RightHandSolver_Target.anchor = LeftHandSolver_Target.anchor;
            LeftHandSolver_Target.anchor = tempAnchor;
            handsInversed = true;
        }
        else if ((mopRotationZ <= 90f || mopRotationZ >= 270f) && handsInversed)
        {
            // Inversed hands back
            Transform tempAnchor = RightHandSolver_Target.anchor;
            RightHandSolver_Target.anchor = LeftHandSolver_Target.anchor;
            LeftHandSolver_Target.anchor = tempAnchor;
            handsInversed = false;
        }
    }
}
