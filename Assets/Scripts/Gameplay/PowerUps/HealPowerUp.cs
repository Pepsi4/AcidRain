public class HealPowerUp : PowerUp
{
    public override void ActivateBoost() {

        if (powerUpConfig is HealPowerUpConfig healConfig)
        {
            PowerUpMediator.PlayerHealth.Heal(healConfig.Heal);
        }
    }
}
