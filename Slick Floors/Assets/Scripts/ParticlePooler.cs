using UnityEngine;
using UnityEngine.Pool;
using System.Collections;

public class ParticlePooler : MonoBehaviour
{
    public static ParticlePooler Instance { get; private set; }

    [SerializeField] private GameObject bubbleParticlePrefab;
    [SerializeField] private int initialPoolSize = 10;
    [SerializeField] private int maxPoolSize = 50;

    private IObjectPool<GameObject> particlePool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        particlePool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, false, initialPoolSize, maxPoolSize);
    }

    private GameObject CreatePooledItem()
    {
        return Instantiate(bubbleParticlePrefab);
    }

    private void OnTakeFromPool(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void OnReturnedToPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void OnDestroyPoolObject(GameObject obj)
    {
        Destroy(obj);
    }

    public void SpawnParticle(Vector3 position, Quaternion rotation)
    {
        GameObject particle = particlePool.Get();
        particle.transform.position = position;
        particle.transform.rotation = rotation;
        StartCoroutine(ReturnToPoolAfterDuration(particle, 2f));
    }

    private IEnumerator ReturnToPoolAfterDuration(GameObject obj, float duration)
    {
        yield return new WaitForSeconds(duration);
        particlePool.Release(obj);
    }
}