using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private DamageTakenEventSO onDamageTaken;
    [SerializeField] private Image bar;
    [SerializeField] private Sprite completedBarSprite;
    [SerializeField] private AudioClip vomitClip;

    private void UpdateHealthBar(DamageTakenEventPayload payload)
    {
        bar.fillAmount = payload.playerHp / payload.playerMaxHp;

        if (bar.fillAmount == 1)
        {
            SoundFXManager.Instance.playSoundFXClip(vomitClip, transform, volume: 1.5f);
            bar.sprite = completedBarSprite;
        }
    }

    private void OnEnable() =>
        onDamageTaken.RegisterListener(UpdateHealthBar);

    private void OnDisable() =>
        onDamageTaken.UnregisterListener(UpdateHealthBar);
}
