using TMPro;
using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
    [SerializeField] private GenericEventSO onTileChangedEvent;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void OnEnable() =>
        onTileChangedEvent.RegisterListener(UpdateScore);

    private void OnDisable() =>
        onTileChangedEvent.RegisterListener(UpdateScore);

    public void UpdateScore(GameEventPayload payload) =>
        scoreText.text = $"Score: {TileHandler.GetTileTypeCount(GroundType.Clean)}";
}
