using System.Collections;
using UnityEngine;

public class Puddle : MonoBehaviour
{
    private float lifetime;
    private bool isPlayerInPuddle;
    private bool isSpeedModifierApplied;
    private Coroutine damageCoroutine;
    private PowerUpMediator powerUpMediator;
    private PlayerController playerController;

    [SerializeField] private int damagePerSecond = 1;
    [SerializeField] private float slowDownFactor = 0.75f;
    private const float DamageEverySeconds = 1f;

    public void Initialize(Vector3 position, float lifetime, int damagePerSecond, PowerUpMediator powerUpMediator)
    {
        transform.position = position;
        this.lifetime = lifetime;
        this.powerUpMediator = powerUpMediator;
        this.damagePerSecond = damagePerSecond;
        gameObject.SetActive(true);
        StartCoroutine(LifetimeCoroutine());
    }

    private IEnumerator LifetimeCoroutine()
    {
        yield return new WaitForSeconds(lifetime);

        if (isPlayerInPuddle)
            OnExitPuddleHandler(playerController);

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController playerController))
        {
            isPlayerInPuddle = true;
            this.playerController = playerController;
            damageCoroutine = StartCoroutine(InflictDamage(playerController.PlayerHealth));

            if (!powerUpMediator.AntiAcidBoots.IsBoostActive)
            {
                playerController.AddSpeedModifier(slowDownFactor);
                isSpeedModifierApplied = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerController playerController))
        {
            OnExitPuddleHandler(playerController);
        }
    }

    private void OnExitPuddleHandler(PlayerController playerController)
    {
        if (isPlayerInPuddle)
        {
            if (isSpeedModifierApplied) 
            {
                playerController.RemoveSpeedModifier(slowDownFactor);
                isSpeedModifierApplied = false; 
            }

            isPlayerInPuddle = false;

            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
            }
        }
    }

    private IEnumerator InflictDamage(PlayerHealth playerHealth)
    {
        while (isPlayerInPuddle)
        {
            if (!powerUpMediator.AntiAcidBoots.IsBoostActive)
            {
                Debug.Log("Player Took damage from puddle");
                playerHealth.TakeDamage(damagePerSecond);
            }
            else
            {
                Debug.Log("AntiAcid Boots Active - No Damage");
            }
            yield return new WaitForSeconds(DamageEverySeconds);
        }
    }
}
