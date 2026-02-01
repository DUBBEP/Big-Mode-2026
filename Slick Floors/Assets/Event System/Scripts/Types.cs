using UnityEngine;

public class GameEventPayload
{

}

public class DamageTakenEventPayload
{
    public float playerHp = 0;
    public float playerMaxHp = 0;
    public DamageSource damageSource = null;
}