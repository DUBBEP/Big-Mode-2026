using UnityEngine;

public class Instakill : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        RemoteDeath.Instance.RemoteDie(0.1f);    
    }
}
