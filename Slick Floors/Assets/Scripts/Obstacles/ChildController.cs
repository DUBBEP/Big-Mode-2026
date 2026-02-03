using Unity.Cinemachine;
using UnityEngine;

public class ChildController : MonoBehaviour
{
    private CinemachineCamera cinemachineCamera;
    private Transform playerTransform;
    private Animator animator;

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
                playerTransform = obj.transform;
                break;
            }
        }
        if (playerTransform == null)
        {
            Debug.LogWarning("ChildController: No GameObject with Player layer found in the scene.");
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("ChildController detected collision with: " + collision.gameObject.name);
        if (animator == null) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Mop"))
        {
            animator.SetBool("Slipped", true);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Objects"))
        {
            animator.SetBool("SignPlaced", true);
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (animator == null) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Mop"))
        {
            if (!animator.GetBool("Slipped"))
                animator.SetBool("Slipped", true);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Objects"))
        {
            if (!animator.GetBool("SignPlaced"))
                animator.SetBool("SignPlaced", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("ChildController detected exit from: " + collision.gameObject.name);
        if (animator == null) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Mop"))
        {
            animator.SetBool("Slipped", false);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Objects"))
        {
            animator.SetBool("SignPlaced", false);
        }
    }

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
    }
}
