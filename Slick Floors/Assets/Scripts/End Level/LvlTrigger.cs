using UnityEngine;
using UnityEngine.SceneManagement;

public class LvlTrigger : MonoBehaviour
{

    [SerializeField] private string sceneToLoad;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LevelScoreCalculator.Instance == null)
            Debug.LogError("Score Calculator is null");
        else
            LevelScoreCalculator.Instance.CalculateFinalScores();

        SceneManager.LoadScene(sceneToLoad);
    }
}
