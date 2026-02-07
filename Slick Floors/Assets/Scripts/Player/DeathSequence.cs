using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathSequence : MonoBehaviour
{
    [SerializeField] private GenericEventSO onPlayerDeath;
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private float launchForce;
    [SerializeField] private float slowdownLength;
    private float slowdownTimer;
    private void StartDeathSequence(GameEventPayload payload)
    {
        StartCoroutine(OnStartDeathSequence());
    }

    private IEnumerator OnStartDeathSequence()
    {
        playerRb.AddForce(Vector2.up * launchForce, ForceMode2D.Impulse);
        playerRb.AddTorque(launchForce, ForceMode2D.Impulse);
        slowdownTimer = slowdownLength;
        while (slowdownTimer > 0.05f)
        {
            slowdownTimer -= Time.deltaTime;

            float slowdownRate = (slowdownTimer / slowdownLength) / 3;
            Debug.Log($"slowdownRate: {slowdownRate}");
            Debug.Log($"Slowdown Timer: {slowdownTimer}");
            Time.timeScale = slowdownRate > 0 ? slowdownRate : 0; 
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            yield return new WaitForFixedUpdate();
        }

        Debug.Log("Restarting Scene");
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnEnable() =>
        onPlayerDeath.RegisterListener(StartDeathSequence);

    private void OnDisable() =>
        onPlayerDeath.UnregisterListener(StartDeathSequence);
}
