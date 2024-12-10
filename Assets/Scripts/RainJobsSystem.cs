using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class RainJobsSystem : MonoBehaviour
{
    [SerializeField] private Raindrop raindropPrefab;

    private NativeArray<float3> raindropPositions;
    private NativeArray<bool> raindropActiveStates;
    private Transform[] raindropTransforms;

    [SerializeField] RainConfig rainConfig;
    private int raindropCount = 1000;
    private float spawnRadius = 10f;
    private float fallSpeed = 5f;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private float playerRadius = 1f;
    [SerializeField] private int damage = 1;
    [SerializeField] private Transform plane;


    private void Start()
    {
        // Ініціалізація масивів
        raindropCount = rainConfig.raindropCount;
        spawnRadius = rainConfig.spawnAreaSize;
        fallSpeed = rainConfig.fallSpeed;

        raindropPositions = new NativeArray<float3>(raindropCount, Allocator.Persistent);
        raindropActiveStates = new NativeArray<bool>(raindropCount, Allocator.Persistent);
        raindropTransforms = new Transform[raindropCount];


        // Створення крапель дощу
        for (int i = 0; i < raindropCount; i++)
        {
            Raindrop raindrop = Instantiate(raindropPrefab, this.transform);
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
            playerRadius = playerRadius,
            damage = damage,
            disappearY = plane.position.y
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
                ResetRaindrop(i);
            }
        }
    }

    public void ResetRaindrop(int index)
    {
        // Краплі спавняться в радіусі навколо гравця
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

    public void DisableRaindrop(int index)
    {
        //raindrop.gameObject.SetActive(false); // Вимикаємо об'єкт
        raindropActiveStates[index] = false; // Повертаємо в пул
    }

    [BurstCompile]
    private struct RaindropMoveJob : IJobParallelFor
    {
        public NativeArray<float3> positions;
        public NativeArray<bool> activeStates;
        public float deltaTime;
        public float fallSpeed;
        public float playerRadius;
        public int damage;
        public float disappearY;

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
