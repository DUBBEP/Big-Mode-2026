using System.ComponentModel;
using UnityEngine;

public class PlayerDamageKnockback : MonoBehaviour
{
    [SerializeField] private DamageTakenEventSO onDamageTaken;
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField][Range(0, 2)] private float upwardsInfluence;

    private void KnockBackPlayer(DamageTakenEventPayload payload)
    {
        Vector2 dir = ((Vector2)payload.damageSource.recievingObject.transform.position - 
            (Vector2)payload.damageSource.sourceObject.transform.position).normalized + (Vector2.up * upwardsInfluence);

        playerRb.AddForce(dir.normalized * payload.damageSource.knockBackForce * 10, ForceMode2D.Impulse);
        Debug.Log("Knocking Player");
    }

    private void OnEnable() =>
        onDamageTaken.RegisterListener(KnockBackPlayer);

    private void OnDisable() =>
        onDamageTaken.UnregisterListener(KnockBackPlayer);
}
