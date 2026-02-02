using UnityEngine;

public class FollowPosition : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    private void Update()
    {
        transform.position = followTarget.position;
    }
}
