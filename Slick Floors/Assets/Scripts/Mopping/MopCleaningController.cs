using UnityEngine;
using UnityEngine.InputSystem;

public class MopCleaningController : MonoBehaviour
{
    [SerializeField] private Collider2D mopTrigger;

    private void Start()
    {
        if (mopTrigger == null) Debug.LogError($"No Mop Trigger set in Mop cleaning controller {name}");
    }

    void Update()
    {
        if (Mouse.current.leftButton.isPressed)
            mopTrigger.enabled = true;
        else
            mopTrigger.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log($"Mop has entered trigger");
        if (collision.TryGetComponent<FloorTile>(out FloorTile tile))
        {
            // Debug.Log($"Mop has found tile");
            tile.ChangeTile(GroundType.Clean);
        }
    }
}