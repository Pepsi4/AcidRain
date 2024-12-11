using UnityEngine;

[CreateAssetMenu(fileName = "PuddleConfig", menuName = "Configs/PuddleConfig")]
public class PuddleConfig : ScriptableObject
{
    public int PuddleCount = 10;
    [Range(0f, 1f)]
    public float PuddleSpawnChance = 0.5f;

    public float MinPuddleLifetime = 10f;

    public float MaxPuddleLifetime = 30f;

    public float PuddleDistanceThreshold = 1.5f;

    public int DamagePerSecond = 1;

    public float SpawnY = 0.1f;
}
