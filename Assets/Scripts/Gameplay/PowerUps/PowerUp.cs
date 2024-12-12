using System;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public virtual void ActivateBoost() { }
    private Action<PowerUp> onTakePowerUp;
    protected PowerUpConfig powerUpConfig;
    protected PowerUpMediator PowerUpMediator { get; private set; }

    public PowerUp Initialize(PowerUpMediator powerUpController, PowerUpConfig config, Action<PowerUp> OnTakePowerUp)
    {
        PowerUpMediator = powerUpController;
        onTakePowerUp = OnTakePowerUp;
        powerUpConfig = config;
        return this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController playerController))
        {
            ActivateBoost();
            onTakePowerUp?.Invoke(this);
        }
    }
}
