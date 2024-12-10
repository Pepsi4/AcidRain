using UnityEngine;

public class BootsPowerUp : PowerUp
{

    public override void ActivateBoost()
    {
        PowerUpMediator.AntiAcidBoots.ResetBoostCharge();
    }
}
