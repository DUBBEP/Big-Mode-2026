using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathSequence : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GenericEventSO onPlayerDeath;
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private Image yellowFade;

    [Header("Sequence Properties")]
    [SerializeField] private float launchForce;
    [SerializeField] private float slowdownLength;

    [Header("Capture")]
    [SerializeField] private CautionImage capture;

    private void Start()
    {
        if (playerRb == null)
            Debug.LogError("playerRb is missing");
    }

    private void StartDeathSequence(GameEventPayload payload)
    {
        StartCoroutine(OnStartDeathSequence());
        StartCoroutine(RespawnCameraController.Instance.ZoomSequence());
    }

    private IEnumerator OnStartDeathSequence()
    {
        playerRb.AddForce(Vector2.up * launchForce, ForceMode2D.Impulse);
        playerRb.AddTorque(launchForce, ForceMode2D.Impulse);

        float yellowTimer = slowdownLength / 1.5f;
        float fadeValue = 0f;

        float slowdownTimer = slowdownLength;
        while (slowdownTimer > 0.05f)
        {
            if (yellowTimer > 0)
                yellowTimer -= Time.deltaTime;
            else
            {
                Mathf.Min(1f, fadeValue += Time.deltaTime);
                yellowFade.color = yellowFade.color + new Color(0,0,0,fadeValue);
            }
            slowdownTimer -= Time.deltaTime;

            float slowdownRate = (slowdownTimer / slowdownLength) / 3;
            Time.timeScale = slowdownRate > 0 ? slowdownRate : 0; 
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            yield return new WaitForFixedUpdate();
        }

        capture.CapturePose();
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        TileHandler.ClearTiles();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnEnable() =>
        onPlayerDeath.RegisterListener(StartDeathSequence);

    private void OnDisable() =>
        onPlayerDeath.UnregisterListener(StartDeathSequence);
}
