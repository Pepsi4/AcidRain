using System;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private Action<PowerUp> onTakePowerUp;
    protected PowerUpMediator PowerUpMediator { get; private set; }
    public virtual void ActivateBoost() { }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController playerController))
        {
            ActivateBoost();
            onTakePowerUp?.Invoke(this);
        }
    }

    public PowerUp Initialize(PowerUpMediator powerUpController, Action<PowerUp> OnTakePowerUp)
    {
        PowerUpMediator = powerUpController;
        onTakePowerUp = OnTakePowerUp;
        return this;
    }
}
