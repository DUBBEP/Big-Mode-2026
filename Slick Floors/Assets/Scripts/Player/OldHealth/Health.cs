using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private GenericEventSO playerDeathEvent;
    [SerializeField] private DamageTakenEventSO damageTakenEvent;

    [SerializeField] private float maxHp;
    [SerializeField] private float invulnerabilityLength;
    
    private float hp;
    private float invulnerabilityTimer;
    private bool invulnerable;
    public float MaxHp {  get { return maxHp; } private set { } }
    public float Hp { get { return hp; } private set { } }

    void Start()
    {
        hp = maxHp;
    }

    private void Update()
    {
        if (invulnerable)
        {
            invulnerabilityTimer -= Time.deltaTime;

            if (invulnerabilityTimer < 0) invulnerable = false;
        }
    }

    public void TakeDamage(DamageSource src)
    {
        if (invulnerable)
        {
            Debug.LogWarning("Player is invulnerable, Damage Ignored");
            return;
        }

        hp -= src.value;
        damageTakenEvent.Raise(new DamageTakenEventPayload
        {
            playerHp = hp,
            playerMaxHp = maxHp,
            damageSource = src,
        });
        if (hp <= 0)
        {
            hp = 0;
            Die();
        }

        invulnerable = true;
        invulnerabilityTimer = invulnerabilityLength;
    }

    private void Die()
    {
        playerDeathEvent.Raise(new GameEventPayload());
    }
}
