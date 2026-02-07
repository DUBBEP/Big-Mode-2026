using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Cinemachine;
using UnityEngine;

public class ChildController : MonoBehaviour
{
    [SerializeField] private AudioClip slipSoundFXClip;
    [SerializeField] private AudioClip reportSoundFXClip;
    [SerializeField] private AudioClip dial911SoundFXClip;
    [SerializeField] private AudioClip nopeSoundFXClip;
    [SerializeField] private AudioClip evilLaughSoundFXClip;
    [SerializeField] private AudioClip pissedOffSoundFXClip;
    [SerializeField] private StudentSlippedEventSO studentSlippedEvent;

    private CinemachineCamera cinemachineCamera;
    private Transform playerTransform;
    private Animator animator;
    private bool hasSlipped = false;
    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("ChildController: No Animator component found on this GameObject.");
        }

        // Find CinemachineCamera in the scene
        cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();
        if (cinemachineCamera == null)
        {
            Debug.LogWarning("ChildController: No CinemachineCamera found in the scene.");
        }

        // Find the first GameObject with the Player layer
        int playerLayer = LayerMask.NameToLayer("Player");
        GameObject[] allObjects = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == playerLayer)
            {
                // Find the child named "body" under this object
                Transform bodyTransform = obj.transform.Find("body");
                if (bodyTransform != null)
                {
                    playerTransform = bodyTransform;
                    break;
                }
            }
        }
        if (playerTransform == null)
        {
            Debug.LogWarning("ChildController: No GameObject with Player layer found in the scene.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // checkSlipped(collision);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Objects"))
        {
            Debug.Log("ChildController: Sign placed!");
            if (!animator.GetBool("SignPlaced"))
            {
                animator.SetBool("SignPlaced", true);
                if (animator.GetBool("Slipped"))
                {
                    playNopeSound();
                }
            }
        }
    }

    public void checkSlipped(Collider2D collision)
    {
        if (animator == null) return;

        // Keep checking if we're on a Clean floor tile
        if (collision.TryGetComponent<FloorTile>(out FloorTile tile))
        {
            if (tile.CurrentType == GroundType.Clean && !animator.GetBool("Slipped"))
            {
                animator.SetBool("Slipped", true);

                // Raise event only once per child
                if (!hasSlipped && studentSlippedEvent != null)
                {
                    hasSlipped = true;
                    studentSlippedEvent.Raise(new StudentSlippedEventPayload { childController = this });
                }
            }
        }

    }

    /*
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (animator == null) return;

        else if (collision.gameObject.layer == LayerMask.NameToLayer("Objects"))
        {
            Debug.Log("ChildController: Sign UN-placed!");
            animator.SetBool("SignPlaced", false);
        }
    }
    */

    private void trackCamera()
    {
        if (cinemachineCamera != null)
            cinemachineCamera.Follow = this.transform;
    }

    private void reportPlayer()
    {
        Debug.Log("ChildController: Player has been reported!");
        // Implement reporting logic here
        // show ragdoll player
        if (cinemachineCamera != null && playerTransform != null)
            cinemachineCamera.Follow = playerTransform;
        // Enter end game sequence
        RemoteDeath.Instance.RemoteDie(1f);
    }

    private void dial911()
    {
        if (animator.GetBool("SignPlaced"))
        {
            return;
        }
        SoundFXManager.Instance.playSoundFXClip(dial911SoundFXClip, this.transform);
    }

    private void playSlipSound()
    {
        SoundFXManager.Instance.playSoundFXClip(slipSoundFXClip, this.transform);
    }

    private void playReportSound()
    {
        SoundFXManager.Instance.playSoundFXClip(reportSoundFXClip, this.transform);
        playEvilLaughSound();
    }

    private void playNopeSound()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("phone") || stateInfo.IsName("phoneloop"))
        {
            return;
        }
        SoundFXManager.Instance.playSoundFXClip(nopeSoundFXClip, this.transform, volume: 2.5f);
    }

    private void playEvilLaughSound()
    {
        SoundFXManager.Instance.playSoundFXClip(evilLaughSoundFXClip, this.transform);
    }

    private void playPissedOffSound()
    {
        SoundFXManager.Instance.playSoundFXClip(pissedOffSoundFXClip, this.transform);
    }
}
