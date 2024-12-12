using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class RainJobsSystem : MonoBehaviour
{
    [SerializeField] private Raindrop raindropPrefab;
    [SerializeField] private PuddleSpawner puddleSpawner;
    [SerializeField] private RainConfig rainConfig;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Transform plane;

    private NativeArray<float3> raindropPositions;
    private NativeArray<bool> raindropsMoveStatus;
    private NativeArray<bool> raindropSimulated;
    private Transform[] raindropTransforms;
    private int raindropCount;
    private float spawnRadius;
    private float fallSpeed;

    public void DisableRaindropMovementControll(int index)
    {
        raindropSimulated[index] = false;
    }

    public void EnableRaindropMovementControll(int index)
    {
        raindropSimulated[index] = true;
        ResetRaindrop(index);
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

        raindropsMoveStatus[index] = true;
    }

    private void Start()
    {
        raindropCount = rainConfig.RaindropCount;
        spawnRadius = rainConfig.SpawnAreaSize;
        fallSpeed = rainConfig.FallSpeed;

        raindropPositions = new NativeArray<float3>(raindropCount, Allocator.Persistent);
        raindropsMoveStatus = new NativeArray<bool>(raindropCount, Allocator.Persistent);
        raindropSimulated = new NativeArray<bool>(raindropCount, Allocator.Persistent);
        raindropTransforms = new Transform[raindropCount];

        for (int i = 0; i < raindropCount; i++)
        {
            Raindrop raindrop = Instantiate(raindropPrefab, transform);
            raindrop.Initialize(this, rainConfig.RaindropDamage, i);
            raindropTransforms[i] = raindrop.transform;
            raindropSimulated[i] = true;
            ResetRaindrop(i);
        }
    }

    private void Update()
    {
        var moveJob = new RaindropMoveJob
        {
            Positions = raindropPositions,
            ActiveStates = raindropsMoveStatus,
            DeltaTime = Time.deltaTime,
            FallSpeed = fallSpeed,
            DisappearY = plane.position.y
        };

        JobHandle jobHandle = moveJob.Schedule(raindropCount, 64);
        jobHandle.Complete();

        for (int i = 0; i < raindropCount; i++)
        {
            if (raindropSimulated[i] == false) continue;

            if (raindropsMoveStatus[i])
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

    private void OnDestroy()
    {
        raindropPositions.Dispose();
        raindropsMoveStatus.Dispose();
    }

    [BurstCompile]
    private struct RaindropMoveJob : IJobParallelFor
    {
        public NativeArray<float3> Positions;
        public NativeArray<bool> ActiveStates;
        public float DeltaTime;
        public float FallSpeed;
        public float DisappearY;

        public void Execute(int index)
        {
            if (!ActiveStates[index]) return;

            float3 position = Positions[index];
            position.y -= FallSpeed * DeltaTime;

            if (position.y < DisappearY)
            {
                ActiveStates[index] = false;
            }
            else
            {
                Positions[index] = position;
            }
        }
    }
}
