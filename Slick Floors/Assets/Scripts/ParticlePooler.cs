using UnityEngine;
using UnityEngine.Pool;
using System.Collections;

public class ParticlePooler : MonoBehaviour
{
    public static ParticlePooler Instance { get; private set; }

    [SerializeField] private GameObject bubbleParticlePrefab;
    [SerializeField] private int initialPoolSize = 10;
    [SerializeField] private int maxPoolSize = 50;

    private ObjectPool<GameObject> particlePool;
    private int totalObjectsInExistence = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        particlePool = new ObjectPool<GameObject>(
            CreatePooledItem, 
            OnTakeFromPool, 
            OnReturnedToPool, 
            OnDestroyPoolObject, 
            true, 
            initialPoolSize, 
            maxPoolSize
        );
    }

    private GameObject CreatePooledItem()
    {  
        totalObjectsInExistence++;
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
        totalObjectsInExistence--;
        Destroy(obj);
    }

    public void SpawnParticle(Vector3 position, Quaternion rotation)
    {
        if (particlePool.CountInactive == 0 && totalObjectsInExistence >= maxPoolSize) 
        {
            return;
        }

        GameObject particle = particlePool.Get();
        particle.transform.position = position;
        particle.transform.rotation = rotation;

        float duration = 2f;
        if (particle.TryGetComponent<ParticleSystem>(out var ps))
        {
            duration = ps.main.duration + ps.main.startLifetime.constantMax;
        }

        StartCoroutine(ReturnToPoolAfterDuration(particle, duration));
    }

    private IEnumerator ReturnToPoolAfterDuration(GameObject obj, float duration)
    {
        yield return new WaitForSeconds(duration);
        
        if (obj != null && obj.activeSelf)
        {
            particlePool.Release(obj);
        }
    }
}