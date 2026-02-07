using UnityEngine;

public class DisgustController : MonoBehaviour
{
    [SerializeField] private GenericEventSO playerDeathEvent;
    [SerializeField] private DamageTakenEventSO disgustRecievedEvent;
    [SerializeField] private float disgustResistance;
    [SerializeField] private float invulnerabilityLength;

    private float digustValue;
    private float invulnerabilityTimer;
    private bool invulnerable;
    public float DisgustResistance { get { return disgustResistance; } private set { } }
    public float DigustValue { get { return digustValue; } private set { } }

    void Start()
    {
        digustValue = 0;
    }

    private void Update()
    {
        if (invulnerable)
        {
            invulnerabilityTimer -= Time.deltaTime;
            if (invulnerabilityTimer < 0) invulnerable = false;
        }
    }

    public void AddDisgust(DamageSource src)
    {
        if (invulnerable)
        {
            Debug.LogWarning("Player is invulnerable, Damage Ignored");
            return;
        }

        digustValue += src.value;
        disgustRecievedEvent.Raise(new DamageTakenEventPayload
        {
            playerHp = digustValue,
            playerMaxHp = disgustResistance,
            damageSource = src,
        });
        if (digustValue >= disgustResistance)
        {
            digustValue = disgustResistance;
            Die();
        }

        if (src.giveInvulnerability)
        {
            invulnerable = true;
            invulnerabilityTimer = invulnerabilityLength;
        }
    }

    private void Die()
    {
        playerDeathEvent.Raise(new GameEventPayload());
    }
}
