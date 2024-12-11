using UnityEngine;

[CreateAssetMenu(fileName = "RainConfig", menuName = "Configs/RainConfig")]
public class RainConfig : ScriptableObject
{
    public int raindropCount = 1000;
    public int puddleCount = 25;
    public float spawnAreaSize = 10f;
    public float fallSpeed = 5f;
    public int RaindropDamage = 1;
}
