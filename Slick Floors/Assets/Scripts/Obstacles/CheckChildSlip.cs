using UnityEngine;

public class CheckChildSlip : MonoBehaviour
{
    [SerializeField] private ChildController childController;

    void Start()
    {
        if (childController == null)
        {
            Debug.LogError("CheckChildSlip: ChildController reference is not set.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (childController != null)
        {
            childController.checkSlipped(collision);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (childController != null)
        {
            childController.checkSlipped(collision);
        }
    }
}
