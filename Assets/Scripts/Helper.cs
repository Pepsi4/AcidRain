using UnityEngine;

public static class Helper
{
    public static Vector2 RandomPointInAnnulus(Vector2 origin, float minRadius, float maxRadius)
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float minRadius2 = minRadius * minRadius;
        float maxRadius2 = maxRadius * maxRadius;
        float randomDistance = Mathf.Sqrt(Random.value * (maxRadius2 - minRadius2) + minRadius2);
        return origin + randomDirection * randomDistance;
    }
}
