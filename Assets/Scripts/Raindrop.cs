using UnityEngine;

public class Raindrop : MonoBehaviour
{
    private int damage = 1;
    private RainJobsSystem rainJobsSystem;
    public int Index { get; private set; }
    public void Initialize(RainJobsSystem system, int damage, int index)
    {
        rainJobsSystem = system;
        this.damage = damage;
        Index = index;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.TakeDamage(damage);
            Debug.Log($"Player takes {damage} damage!");
            rainJobsSystem.DisableRaindrop(Index);
        }
    }
}
