using UnityEngine;

public class PlayerDamageKnockback : MonoBehaviour
{
    [SerializeField] private DamageTakenEventSO onDamageTaken;
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField][Range(0, 2)] private float upwardsInfluence;

    private void KnockBackPlayer(DamageTakenEventPayload payload)
    {
        if (payload.damageSource.knockBackForce <= 0) return;

        if (payload.damageSource.sourceObject == null)
            KnockPlayerUpward(payload);
        else
            KnockAwayFromObject(payload);
    }

    private void KnockAwayFromObject(DamageTakenEventPayload payload)
    {
        Vector2 dir = ((Vector2)payload.damageSource.recievingObject.transform.position -
            (Vector2)payload.damageSource.sourceObject.transform.position).normalized + (Vector2.up * upwardsInfluence);

        playerRb.AddForce(dir.normalized * payload.damageSource.knockBackForce * 10, ForceMode2D.Impulse);
        Debug.Log("Knocking Player");
    }
    private void KnockPlayerUpward(DamageTakenEventPayload payload)
    {
        playerRb.AddForce(Vector2.up * payload.damageSource.knockBackForce, ForceMode2D.Impulse);
    }

    private void OnEnable() =>
        onDamageTaken.RegisterListener(KnockBackPlayer);

    private void OnDisable() =>
        onDamageTaken.UnregisterListener(KnockBackPlayer);
}
