using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LevelScoreCalculator.Instance == null)
            Debug.LogError("Score Calculator is null");
        else
            LevelScoreCalculator.Instance.CalculateFinalScores();

        SceneManager.LoadScene("Level Finished");
    }
}
