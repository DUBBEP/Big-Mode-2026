using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnCameraController : MonoBehaviour
{
    public static RespawnCameraController Instance;

    [SerializeField] private float zoomInTime = 3f;
    [SerializeField] private float zoomOutTime = 3f;
    [SerializeField] private float maxZoomOrthoZoom = 1.5f;
    [SerializeField] private float minZoomOrthoZoom = 8f;
    [SerializeField] private float zoomDampening = 0.2f;
    [SerializeField] private float orthoInRate = 30f;
    [SerializeField] private float orthoOutRate = 30f;

    private CinemachineCamera vCam;
    private CinemachinePositionComposer posComp;

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
        if (FindFirstObjectByType<CinemachineCamera>() != null)
            vCam = FindFirstObjectByType<CinemachineCamera>();
        else
            Debug.LogError("Could Not Find camera Component");
        if (FindFirstObjectByType<CinemachinePositionComposer>() != null)
            posComp = FindFirstObjectByType<CinemachinePositionComposer>();
        else
            Debug.LogError("Could Not Find position composer Component");

    }

    public void MoveCamera(Transform target, float zoom)
    {
        if (vCam == null) vCam = FindFirstObjectByType<CinemachineCamera>();
        if (posComp == null) posComp = FindFirstObjectByType<CinemachinePositionComposer>();

        vCam.Follow = target;
        vCam.Lens.OrthographicSize = zoom;
        Debug.Log($"ortho {vCam.Lens.OrthographicSize}");
        vCam.ForceCameraPosition(target.position, Quaternion.identity);
        vCam.OnTargetObjectWarped(target, target.position - vCam.transform.position);
    }
    
    public IEnumerator ZoomSequence(Transform followTarget = null, ZoomType zType = ZoomType.In)
    {
        Vector2 dzSize = posComp.Composition.DeadZone.Size;
        posComp.Composition.DeadZone.Size = Vector2.zero;
        posComp.Damping = Vector3.one * zoomDampening;
        float timer = 0;

        if (followTarget != null)
            vCam.Follow = followTarget;

        float targetOrtho;
        float endZoomTime;
        float targetOrthoRate;
        if (zType == ZoomType.In)
        {
            endZoomTime = zoomInTime;
            targetOrtho = maxZoomOrthoZoom;
            targetOrthoRate = orthoInRate;
        }
        else
        {
            posComp.Damping = Vector3.one * zoomDampening;
            endZoomTime = zoomOutTime;
            targetOrtho = minZoomOrthoZoom;
            targetOrthoRate = orthoOutRate;
            yield return new WaitForSecondsRealtime(0.7f);
        }

        while (timer < endZoomTime)
        {
            // apply dampening and ortho size change
            vCam.Lens.OrthographicSize = Mathf.MoveTowards(
                vCam.Lens.OrthographicSize,
                targetOrtho,
                targetOrthoRate * 0.1f * Time.unscaledDeltaTime
            );

            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        if (zType == ZoomType.Out)
            vCam.Follow = FindFirstObjectByType<PlayerMovement>().transform;

        posComp.Damping = Vector3.zero;
        posComp.Composition.DeadZone.Size = dzSize;
    }

    public enum ZoomType
    {
        In,
        Out,
    }
}
