using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private DamageTakenEventSO onDamageTaken;
    [SerializeField] private Image bar;
        
    private void UpdateHealthBar(DamageTakenEventPayload payload)
    {
        bar.fillAmount = payload.playerHp / payload.playerMaxHp;
        Debug.Log($"damage take payload: {payload.playerHp}");
        Debug.Log($"damage take payload: {payload.player}");
        Debug.Log($"damage take payload: {payload.damageSource.knockBackForce}");
        Debug.Log($"damage take payload: {payload.damageSource.value}");
        Debug.Log($"damage take payload: {payload.damageSource.sourceObject}");
    }

    private void OnEnable() =>
        onDamageTaken.RegisterListener(UpdateHealthBar);

    private void OnDisable() =>
        onDamageTaken.UnregisterListener(UpdateHealthBar);
}
