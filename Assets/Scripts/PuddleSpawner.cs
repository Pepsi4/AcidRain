using UnityEngine;
using Unity.Mathematics;

public class PuddleSpawner : MonoBehaviour
{
    [SerializeField] private Puddle puddlePrefab;
    [SerializeField] private PuddleConfig puddleConfig;
    [field: SerializeField] public PowerUpMediator PowerUpMediator { get; private set; }

    private Transform[] puddleTransforms;
    private int puddleCount;

    private void Start()
    {
        puddleCount = puddleConfig.PuddleCount;
        puddleTransforms = new Transform[puddleCount];

        for (int i = 0; i < puddleCount; i++)
        {
            puddleTransforms[i] = Instantiate(puddlePrefab, transform).gameObject.transform;
            puddleTransforms[i].gameObject.SetActive(false);
        }
    }

    public void TrySpawnPuddle(Vector3 position)
    {
        if (UnityEngine.Random.value > puddleConfig.PuddleSpawnChance)
        {
            return;
        }

        for (int i = 0; i < puddleCount; i++)
        {
            if (puddleTransforms[i].gameObject.activeSelf)
            {
                float distance = Vector3.Distance(puddleTransforms[i].position, position);
                if (distance < puddleConfig.PuddleDistanceThreshold)
                {
                    return;
                }
            }
        }

        for (int i = 0; i < puddleCount; i++)
        {
            if (!puddleTransforms[i].gameObject.activeSelf)
            {
                Puddle puddle = puddleTransforms[i].GetComponent<Puddle>();
                puddle.Initialize(new Vector3(position.x, puddleConfig.SpawnY, position.z), UnityEngine.Random.Range(puddleConfig.MinPuddleLifetime, puddleConfig.MaxPuddleLifetime), puddleConfig.DamagePerSecond, PowerUpMediator);
                return;
            }
        }
    }
}
