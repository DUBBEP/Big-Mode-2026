using UnityEngine;

public class Spike : MonoBehaviour, IDamageSource
{
    [SerializeField] private float damageValue;
    [SerializeField] private float knockBackForce;

    public DamageSource GetDamageSource()
    {
        return new DamageSource
        {
            knockBackForce = knockBackForce,
            value = damageValue,
            sourceObject = gameObject,
        };
    }
}
