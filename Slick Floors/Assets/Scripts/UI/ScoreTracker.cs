using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;


public class ScoreTracker : MonoBehaviour
{
    [SerializeField] private GenericEventSO onTileChangedEvent;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image scoreBar;

    private void OnEnable() =>
        onTileChangedEvent.RegisterListener(UpdateScore);

    private void OnDisable() =>
        onTileChangedEvent.RegisterListener(UpdateScore);

    public void UpdateScore(GameEventPayload payload)
    {
        float score = TileHandler.GetTileTypeCount(GroundType.Clean) / (float)TileHandler.TotalTileCount;

        if (scoreBar == null) return;

        scoreBar.fillAmount = score;

        if (score >= 1.0f)
        {
            scoreText.text = $"<palette><shake>{score:P0}";
        }
        else if (score >= 0.75f)
        {
            scoreText.text = $"<wave>{score:P0}";
        }
        else if (score >= 0.5f)
        {
            scoreText.text = $"<funky>{score:P0}";
        }
        else if (score >= 0.25f)
        {
            scoreText.text = $"<shear>{score:P0}";
        }
        else
        {
            scoreText.text = $"{score:P0}";
        }
    }
}
