using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathSequence : MonoBehaviour
{
    [SerializeField] private GenericEventSO onPlayerDeath;
    [SerializeField] private float slowdownLength;
    private float slowdownTimer;
    private void StartDeathSequence(GameEventPayload payload)
    {
        StartCoroutine(OnStartDeathSequence());
    }

    private IEnumerator OnStartDeathSequence()
    {
        slowdownTimer = slowdownLength;
        while (slowdownTimer > 0.05f)
        {
            slowdownTimer -= Time.deltaTime;

            float slowdownRate = (slowdownTimer / slowdownLength) / 3;
            Debug.Log($"slowdownRate: {slowdownRate}");
            Debug.Log($"Slowdown Timer: {slowdownTimer}");
            Time.timeScale = slowdownRate > 0 ? slowdownRate : 0; 
             
            yield return new WaitForFixedUpdate();
        }

        Debug.Log("Restarting Scene");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnEnable() =>
        onPlayerDeath.RegisterListener(StartDeathSequence);

    private void OnDisable() =>
        onPlayerDeath.UnregisterListener(StartDeathSequence);
}
