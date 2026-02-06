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
    public float finalTime;
    public float finalTimeScore;
    public float tileTotal;
    public float cleanTitles;
    public float neutralTiles;
    public float dirtyTiles;
    public float tilePercentage;
    public float studentCount;
}

public enum GroundType
{
    Dirty,
    Neutral,
    Clean,
}