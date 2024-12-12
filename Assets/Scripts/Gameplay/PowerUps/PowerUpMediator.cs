using UnityEngine;

public class PowerUpMediator : MonoBehaviour
{
    [field: SerializeField] public AntiAcidBoots AntiAcidBoots { get; private set; }
    [field: SerializeField] public Shield Shield { get; private set; }
    [field: SerializeField] public PlayerHealth PlayerHealth { get; private set; }
}
