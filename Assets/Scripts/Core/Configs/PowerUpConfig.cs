using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpConfig", menuName = "Configs/PowerUpConfig")]
public class PowerUpConfig : ScriptableObject
{
    public PowerUp PowerUpPrefab;
    public float MinDistance = 5f;
    public float MaxDistance = 10f;
    public float SpawnInterval = 5f;
    public float SpawnY;
}
