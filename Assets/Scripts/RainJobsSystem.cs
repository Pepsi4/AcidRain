using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class RainJobsSystem : MonoBehaviour
{
    [SerializeField] private GameObject raindropPrefab;
    [SerializeField] private int raindropCount = 1000;
    [SerializeField] private float spawnAreaSize = 10f;
    [SerializeField] private float fallSpeed = 5f;

    private NativeArray<float3> raindropPositions;
    private NativeArray<bool> raindropActiveStates;
    private Transform[] raindropTransforms;

    private void Start()
    {
        // Ініціалізація крапель
        raindropPositions = new NativeArray<float3>(raindropCount, Allocator.Persistent);
        raindropActiveStates = new NativeArray<bool>(raindropCount, Allocator.Persistent);
        raindropTransforms = new Transform[raindropCount];

        for (int i = 0; i < raindropCount; i++)
        {
            GameObject raindrop = Instantiate(raindropPrefab);
            raindropTransforms[i] = raindrop.transform;
            ResetRaindrop(i);
        }
    }

    private void Update()
    {
        // Створення Job для переміщення крапель
        var moveJob = new RaindropMoveJob
        {
            positions = raindropPositions,
            activeStates = raindropActiveStates,
            deltaTime = Time.deltaTime,
            fallSpeed = fallSpeed
        };

        // Запуск Job
        JobHandle jobHandle = moveJob.Schedule(raindropCount, 64);
        jobHandle.Complete();

        // Оновлення позицій у світі Unity
        for (int i = 0; i < raindropCount; i++)
        {
            if (raindropActiveStates[i])
            {
                raindropTransforms[i].position = raindropPositions[i];
            }
            else
            {
                ResetRaindrop(i);
            }
        }
    }

    private void ResetRaindrop(int index)
    {
        raindropPositions[index] = new float3(
            UnityEngine.Random.Range(-spawnAreaSize, spawnAreaSize),
            UnityEngine.Random.Range(5f, 10f),
            UnityEngine.Random.Range(-spawnAreaSize, spawnAreaSize)
        );
        raindropActiveStates[index] = true;
    }

    private void OnDestroy()
    {
        // Звільнення пам'яті
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

        public void Execute(int index)
        {
            if (!activeStates[index]) return;

            float3 position = positions[index];
            position.y -= fallSpeed * deltaTime;

            if (position.y < 0f)
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
