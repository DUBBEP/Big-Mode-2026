using UnityEngine;

public class CautionImage : MonoBehaviour
{
    [Tooltip("The SpriteRenderer to assign the captured sprite to.")]
    [SerializeField] private SpriteRenderer targetRenderer;

    [Tooltip("The root object of the player to capture.")]
    [SerializeField] private Transform playerRoot;

    [Tooltip("Layer mask to include in the capture (e.g. Player layer).")]
    [SerializeField] private LayerMask captureLayer = -1; // Default to Everything

    [Header("Capture Settings")]
    [SerializeField] private float padding = 0.5f;
    [SerializeField] private float pixelsPerUnit = 100f;
    [Tooltip("Scale multiplier for the generated sprite size.")]
    [SerializeField] private float imageScale = 1f;
    [SerializeField] private bool captureNow = false;

    private void Start()
    {
        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<SpriteRenderer>();
        }

        if (playerRoot == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerRoot = player.transform;
            }
        }
    }

    private void Update()
    {
        if (captureNow)
        {
            captureNow = false;
            CapturePose();
        }
    }

    [ContextMenu("Capture Pose")]
    public void CapturePose()
    {
        if (playerRoot == null)
        {
            // Try to find player if not assigned
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerRoot = player.transform;

            if (playerRoot == null)
            {
                Debug.LogError("[CautionImage] Player Root not assigned and no object with tag 'Player' found!");
                return;
            }
        }

        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<SpriteRenderer>();
            if (targetRenderer == null)
            {
                Debug.LogError("[CautionImage] Target SpriteRenderer not assigned!");
                return;
            }
        }

        // 1. Calculate Bounds of the player visual parts
        // We look for Renderers (SpriteRenderer, MeshRenderer, etc.)
        var renderers = playerRoot.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            Debug.LogWarning("[CautionImage] Player root has no renderers to capture.");
            return;
        }

        Bounds bounds = new Bounds(renderers[0].bounds.center, Vector3.zero);
        foreach (var r in renderers)
        {
            // Skip particle systems or trails if necessary, but generally we want to capture the look
            bounds.Encapsulate(r.bounds);
        }

        // Expand bounds by padding
        bounds.Expand(padding);

        // 2. Setup a temporary camera
        GameObject camObj = new GameObject("CheckPointCaptureCamera");
        RenderTexture rt = null;

        try
        {
            Camera cam = camObj.AddComponent<Camera>();

            // Ensure the camera is orthographic
            cam.orthographic = true;

            // Set camera properties to isolate the player
            cam.cullingMask = captureLayer;
            cam.clearFlags = CameraClearFlags.Color;
            cam.backgroundColor = new Color(0, 0, 0, 0); // Transparent background
            cam.depth = -100; // Render before main camera (though we call Render() manually)

            // Calculate size directly from bounds
            float verticalSize = bounds.size.y;
            float horizontalSize = bounds.size.x;

            // Orthographic size is half the vertical size
            cam.orthographicSize = verticalSize * 0.5f;

            // Position camera: Center of bounds, backed off in Z
            cam.transform.position = new Vector3(bounds.center.x, bounds.center.y, -10f);

            // 3. Render to RenderTexture
            int width = Mathf.CeilToInt(horizontalSize * pixelsPerUnit);
            int height = Mathf.CeilToInt(verticalSize * pixelsPerUnit);

            // Ensure valid dimensions
            width = Mathf.Max(1, width);
            height = Mathf.Max(1, height);

            rt = RenderTexture.GetTemporary(width, height, 24, RenderTextureFormat.ARGB32);
            cam.targetTexture = rt;

            // Render
            cam.Render();

            // 4. Read pixels into Texture2D
            RenderTexture.active = rt;
            Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();

            // 6. Create and Assign Sprite
            // Pivot at center (0.5, 0.5)
            // Adjust PPU based on imageScale. Higher scale = Lower PPU = Larger Sprite
            float finalPPU = pixelsPerUnit / Mathf.Max(0.001f, imageScale);

            Sprite newSprite = Sprite.Create(tex, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), finalPPU);
            newSprite.name = "PlayerParams_Capture";

            targetRenderer.sprite = newSprite;

            Debug.Log($"[CautionImage] Captured player pose to sprite. Size: {width}x{height}");
        }
        finally
        {
            // 5. Cleanup
            RenderTexture.active = null;
            if (rt != null)
            {
                RenderTexture.ReleaseTemporary(rt);
            }
            if (camObj != null)
            {
                DestroyImmediate(camObj);
            }
        }
    }
}
