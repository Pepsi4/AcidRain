using UnityEngine;

[CreateAssetMenu(fileName = "RainConfig", menuName = "Configs/RainConfig")]
public class RainConfig : ScriptableObject
{
    public int RaindropCount = 1000;
    public int PuddleCount = 25;
    public float SpawnAreaSize = 10f;
    public float FallSpeed = 5f;
    public int RaindropDamage = 1;
}
