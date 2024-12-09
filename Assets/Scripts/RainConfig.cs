using UnityEngine;

[CreateAssetMenu(fileName = "RainConfig", menuName = "Configs/RainConfig")]
public class RainConfig : ScriptableObject
{
    public float spawnInterval = 0.1f;

    public float fallSpeed = 5f;

    public int maxRaindrops = 1000;
}
