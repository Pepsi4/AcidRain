using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class RainJobsSystem : MonoBehaviour
{
    [SerializeField] private Raindrop raindropPrefab;
    [SerializeField] private PuddleSpawner puddleSpawner;

    private NativeArray<float3> raindropPositions;
    private NativeArray<bool> raindropActiveStates;
    private Transform[] raindropTransforms;

    [SerializeField] private RainConfig rainConfig;
    [SerializeField] private PlayerHealth playerHealth;

    private int raindropCount;
    private float spawnRadius;
    private float fallSpeed;

    private void Start()
    {
        raindropCount = rainConfig.raindropCount;
        spawnRadius = rainConfig.spawnAreaSize;
        fallSpeed = rainConfig.fallSpeed;

        raindropPositions = new NativeArray<float3>(raindropCount, Allocator.Persistent);
        raindropActiveStates = new NativeArray<bool>(raindropCount, Allocator.Persistent);
        raindropTransforms = new Transform[raindropCount];

        for (int i = 0; i < raindropCount; i++)
        {
            Raindrop raindrop = Instantiate(raindropPrefab, transform);
            raindrop.Initialize(this, rainConfig.RaindropDamage, i);
            raindropTransforms[i] = raindrop.transform;
            ResetRaindrop(i);
        }
    }

    private void Update()
    {
        var moveJob = new RaindropMoveJob
        {
            positions = raindropPositions,
            activeStates = raindropActiveStates,
            deltaTime = Time.deltaTime,
            fallSpeed = fallSpeed,
            disappearY = playerHealth.transform.position.y
        };

        JobHandle jobHandle = moveJob.Schedule(raindropCount, 64);
        jobHandle.Complete();

        for (int i = 0; i < raindropCount; i++)
        {
            if (raindropActiveStates[i])
            {
                raindropTransforms[i].position = raindropPositions[i];
            }
            else
            {
                puddleSpawner.TrySpawnPuddle(raindropPositions[i]);
                ResetRaindrop(i);
            }
        }
    }

    public void ResetRaindrop(int index)
    {
        float angle = UnityEngine.Random.Range(0f, math.PI * 2);
        float radius = UnityEngine.Random.Range(0f, spawnRadius);

        float x = math.cos(angle) * radius;
        float z = math.sin(angle) * radius;

        raindropPositions[index] = new float3(
            playerHealth.transform.position.x + x,
            UnityEngine.Random.Range(5f, 10f),
            playerHealth.transform.position.z + z
        );

        raindropActiveStates[index] = true;
    }

    private void OnDestroy()
    {
        raindropPositions.Dispose();
        raindropActiveStates.Dispose();
    }

    [BurstCompile]
    private struct RaindropMoveJob : IJobParallelFor
    {
        public NativeArray<float3> positions;
        public NativeArray<bool> activeStates;
        public float deltaTime;
        public float fallSpeed;
        public float disappearY;

        public void Execute(int index)
        {
            if (!activeStates[index]) return;

            float3 position = positions[index];
            position.y -= fallSpeed * deltaTime;

            if (position.y < disappearY)
            {
                activeStates[index] = false;
            }
            else
            {
                positions[index] = position;
            }
        }
    }
}
