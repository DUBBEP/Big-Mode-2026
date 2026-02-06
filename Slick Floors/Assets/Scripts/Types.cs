using UnityEngine;

public class DamageSource
{
    public float value = 0;
    public float knockBackForce = 0;
    public GameObject sourceObject = null;
    public GameObject recievingObject = null;

}

public class LevelResults
{
    public string finalTime;
    public string tilePercentage;
    public string studentCount;
    public string grade;
}

public enum GroundType
{
    Dirty,
    Neutral,
    Clean,
}