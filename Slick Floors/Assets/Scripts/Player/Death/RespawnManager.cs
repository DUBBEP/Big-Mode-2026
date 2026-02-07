using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance;

    [SerializeField] public GameObject CautionSignPrefab;
    [HideInInspector] public Sprite PlayerDeathSprite = null;

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
        sr.sprite = PlayerDeathSprite;
    }
}
