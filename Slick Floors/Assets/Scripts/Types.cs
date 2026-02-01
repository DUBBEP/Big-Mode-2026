using UnityEngine;

public class DamageSource
{
    public float value = 0;
    public float knockBackForce = 0;
    public GameObject sourceObject = null;
    public GameObject recievingObject = null;

}

public enum GroundType
{
    Dirty,
    Neutral,
    Clean,
}