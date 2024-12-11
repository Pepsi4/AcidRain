using UnityEngine;

public class ShieldPowerUp : PowerUp
{
    public override void ActivateBoost()
    {
        PowerUpMediator.Shield.ActivateBoost();
    }
}
