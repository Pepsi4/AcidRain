using System.Collections;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private float shieldLifetime = 10f;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private DamageTypes immuneDamageType = DamageTypes.Raindrop;
    [SerializeField] private ShieldTimer shieldTimer;
    [SerializeField] private GameObject shieldTimerUI;

    private Coroutine shieldCoroutine;
    private bool isShieldActive;


    public void ActivateBoost()
    {
        if (isShieldActive)
            RestartTimer();
        else
            ActivateShield();
    }

    private void ActivateShield()
    {
        isShieldActive = true;
        this.gameObject.SetActive(true);

        StartTimer();
        shieldTimerUI.SetActive(true);

        playerHealth.ImmuneDamageTypes.Add(immuneDamageType);

        if (shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine);
        }
    }

    private void StartTimer()
    {
        shieldTimer.Activate(shieldLifetime);
        shieldTimer.OnTimerEnd += DeactivateShield;
    }

    private void RestartTimer()
    {
        shieldTimer.Activate(shieldLifetime);
    }

    private void DeactivateShield()
    {
        shieldTimerUI.SetActive(false);
        playerHealth.ImmuneDamageTypes.Remove(immuneDamageType);
        isShieldActive = false;
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isShieldActive && other.TryGetComponent(out Raindrop raindrop))
        {
            Debug.Log("Raindrop on shield");
            // HandleRaindropCollision(other.gameObject);
        }
    }

    private void HandleRaindropCollision(GameObject raindrop)
    {
        Rigidbody rb = raindrop.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = new Vector3(Random.Range(-1f, 1f), -1f, Random.Range(-1f, 1f)) * 2f; // Slide off effect
        }
    }
}
