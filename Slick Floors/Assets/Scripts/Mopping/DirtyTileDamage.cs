using UnityEngine;

public class DirtyTileDamage : MonoBehaviour, IDamageSource
{
    [HideInInspector]
    public float damageValue;
    public DamageSource GetDamageSource()
    {
        return new DamageSource
        {
            value = damageValue,
            knockBackForce = 0,
            sourceObject = gameObject,
        };
    }
}
