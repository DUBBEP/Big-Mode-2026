using UnityEngine;

public class RemoteDeath : MonoBehaviour
{
    public static RemoteDeath Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RemoteDie(float delay = 0.2f)
    {
        Invoke(nameof(KillPlayer), delay);
    }


    private void KillPlayer()
    {
        FindFirstObjectByType<DisgustController>().Die();
    }
}
