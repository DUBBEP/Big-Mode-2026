using UnityEngine;
using TMPro;
using Unity.Cinemachine;
public class TutorialTrigger : MonoBehaviour
{
    [Header("Character New And Old")]
    public GameObject playerWithMopPrefab;
    public GameObject playerWithoutMop;
    private bool hasTriggered = false;
    public string nestedPath = "Janitor/body";
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered)
        {
            hasTriggered = true;
            SwapPlayer(playerWithoutMop);
        }
    }

    void SwapPlayer(GameObject oldPlayer)
    {
        Vector3 triggerPos = transform.position - Vector3.up * 1f + Vector3.forward * 1f;
        Quaternion triggerRot = transform.rotation;
        Vector3 playerScale = oldPlayer.transform.localScale;

        GameObject newPlayer = Instantiate(playerWithMopPrefab, triggerPos, triggerRot);
        newPlayer.transform.localScale = playerScale;

        Transform cameraTarget = newPlayer.transform.Find(nestedPath);

        var vcam = GameObject.FindFirstObjectByType<CinemachineCamera>();
        if (vcam != null) 
        {
            vcam.Follow = cameraTarget;
            vcam.LookAt = cameraTarget;
        }

        Destroy(oldPlayer);
        Destroy(gameObject);
    }
}