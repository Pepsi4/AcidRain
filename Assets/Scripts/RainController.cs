using UnityEngine;
using UnityEngine.Pool;

public class RainController : MonoBehaviour
{
    [SerializeField] private GameObject raindropPrefab;
    [SerializeField] private RainConfig rainConfig;

    private ObjectPool<Raindrop> raindropPool;
    private float spawnTimer;

    private void Start()
    {
        raindropPool = new ObjectPool<Raindrop>(
            createFunc: CreateRaindrop,
            actionOnGet: OnRaindropSpawned,
            actionOnRelease: OnRaindropReturned,
            actionOnDestroy: DestroyRaindrop,
            maxSize: rainConfig.maxRaindrops
        );
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= rainConfig.spawnInterval)
        {
            spawnTimer = 0f;
            SpawnRaindrop();
        }
    }

    private Raindrop CreateRaindrop()
    {
        GameObject raindropGO = Instantiate(raindropPrefab);
        Raindrop raindrop = raindropGO.GetComponent<Raindrop>();
        return raindrop;
    }

    private void OnRaindropSpawned(Raindrop raindrop)
    {
        raindrop.gameObject.SetActive(true);
        raindrop.transform.position = new Vector3(
            Random.Range(-10f, 10f),
            Random.Range(10f, 15f),
            Random.Range(-10f, 10f)
        );
        raindrop.Initialize(rainConfig.fallSpeed, ReturnRaindropToPool);
    }

    private void OnRaindropReturned(Raindrop raindrop)
    {
        raindrop.gameObject.SetActive(false);
    }

    private void DestroyRaindrop(Raindrop raindrop)
    {
        Destroy(raindrop.gameObject);
    }

    private void SpawnRaindrop()
    {
        if (raindropPool.CountActive < rainConfig.maxRaindrops)
        {
            raindropPool.Get();
        }
    }

    private void ReturnRaindropToPool(Raindrop raindrop)
    {
        raindropPool.Release(raindrop);
    }
}
