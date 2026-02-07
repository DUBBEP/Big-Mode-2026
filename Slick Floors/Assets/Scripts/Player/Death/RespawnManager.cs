using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance;

    [SerializeField] public GameObject CautionSignPrefab;
    [SerializeField] private float startUpLength;
    [SerializeField] private float startCamZoom;
    [HideInInspector] public Sprite PlayerDeathSprite = null;

    Rigidbody2D signRb;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (PlayerDeathSprite == null)
        {
            Debug.LogWarning("No PlayerDeathSprite in Respawn Manager. This Should Be first Load of Scene.");
            return;
        }
        SpawnSign();
    }

    private void SpawnSign()
    {
        Transform playerTransform = FindFirstObjectByType<PlayerMovement>().transform;
        GameObject newSign = Instantiate(CautionSignPrefab, (Vector2)playerTransform.position + (Vector2.right * 5), Quaternion.identity);
        SpriteRenderer sr = newSign.GetComponent<PlayerImageHolder>().PlayerImageRenderer;
         signRb = newSign.GetComponentInChildren<Rigidbody2D>();
        
        sr.sprite = PlayerDeathSprite;
        signRb.gravityScale = 0f;
        Invoke(nameof(SetSignGrav), 0.5f);

        StartCoroutine(StartUpSequence());
        RespawnCameraController.Instance.MoveCamera(sr.transform, startCamZoom);
        StartCoroutine(RespawnCameraController.Instance.ZoomSequence(null, RespawnCameraController.ZoomType.Out));
    }

    private IEnumerator StartUpSequence()
    {
        float timer = 0.01f;
        Time.timeScale = timer;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        while (timer <= 1f)
        {
            timer += Time.deltaTime;
            float SpeedUpRate = (timer / startUpLength) / 3;
            
            Time.timeScale = SpeedUpRate;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            yield return new WaitForFixedUpdate();
        }

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }


    private void SetSignGrav() => signRb.gravityScale = 1f;
}
