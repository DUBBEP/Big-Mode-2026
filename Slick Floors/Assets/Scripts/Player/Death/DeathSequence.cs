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
    [SerializeField] private float fadeSpeed;

    [Header("Sequence Properties")]
    [SerializeField] private float launchForce = 600;
    [SerializeField] private float slowdownLength = 0.4f;

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

        float slowdownTimer = slowdownLength;
        while (slowdownTimer > 0.5f)
        {
            if (yellowTimer > 0)
                yellowTimer -= Time.unscaledDeltaTime;
            else
            {
                Color c = yellowFade.color;
                c.a = Mathf.MoveTowards(c.a, 1f, Time.unscaledDeltaTime * fadeSpeed);
                yellowFade.color = c;
            }
            slowdownTimer -= Time.unscaledDeltaTime;

            float slowdownRate = (slowdownTimer / slowdownLength) / 3;
            Time.timeScale = slowdownRate > 0 ? slowdownRate : 0; 
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            yield return null;
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
