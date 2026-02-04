using UnityEngine;

public class MopHandle : MonoBehaviour
{
    public FixedObject RightHandSolver_Target;
    public FixedObject LeftHandSolver_Target;
    public Transform mopAngle;
    private bool mopHeld = true;
    public bool handsInversed = false;
    private Transform prevAnchor;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (mopHeld)
        {
            swapHands();
        }
    }

    void swapHands()
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
            // Debug.Log("Mop hands inversed");
        }
        else if ((mopRotationZ <= 90f || mopRotationZ >= 270f) && handsInversed)
        {
            // Inversed hands back
            Transform tempAnchor = RightHandSolver_Target.anchor;
            RightHandSolver_Target.anchor = LeftHandSolver_Target.anchor;
            LeftHandSolver_Target.anchor = tempAnchor;
            handsInversed = false;
            // Debug.Log("Mop hands un-inversed");
        }
    }

    public void holdSign(Transform newAnchor)
    {
        mopHeld = false;
        if (!handsInversed)
        {
            prevAnchor = LeftHandSolver_Target.anchor;
            LeftHandSolver_Target.anchor = newAnchor;
        }
        else
        {
            prevAnchor = RightHandSolver_Target.anchor;
            RightHandSolver_Target.anchor = newAnchor;
        }
    }

    public void releaseSign()
    {
        mopHeld = true;
        if (!handsInversed)
        {
            LeftHandSolver_Target.anchor = prevAnchor;
        }
        else
        {
            RightHandSolver_Target.anchor = prevAnchor;
        }
    }
}
