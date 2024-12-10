using UnityEngine;

[CreateAssetMenu(fileName = "RainConfig", menuName = "Configs/RainConfig")]
public class RainConfig : ScriptableObject
{
    public float spawnAreaSize = 0.1f;

    public float fallSpeed = 5f;

    public int raindropCount = 1000;
    public int RaindropDamage = 1;
}
