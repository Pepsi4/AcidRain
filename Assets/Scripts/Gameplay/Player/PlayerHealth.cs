using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public delegate void HealthChanged(int currentHealth, int maxHealth);
    public event HealthChanged OnHealthChanged;
    public Action OnDie;
    public List<DamageTypes> ImmuneDamageTypes { get; set; } = new List<DamageTypes>();

    [SerializeField] private EndGamePanel endGamePanel;
    [SerializeField] private int maxHealth = 50;
    private int currentHealth;
    private bool isDead;

    [Header("Damage visuals")]
    [SerializeField] private Material playerMaterial;
    [SerializeField] private float flashDamageTime = 0.1f;
    [SerializeField] private Color damageColor;
    [SerializeField] private Color originalColor;
    private const string ColorPropName = "_BaseColor";

    public void TakeDamage(DamageTypes damageType, int damage)
    {
        if (ImmuneDamageTypes.Contains(damageType)) return;

        TakeDamage(damage);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Die()
    {
        if (TryGetComponent(out PlayerController playerController))
        {
            isDead = true;
            OnDie?.Invoke();
            playerController.SetState(new DeadState());
            endGamePanel.Show();
        }
    }

    private IEnumerator FlashRed()
    {
        playerMaterial.SetColor(ColorPropName, damageColor);
        yield return new WaitForSeconds(flashDamageTime);
        playerMaterial.SetColor(ColorPropName, originalColor);
    }
}
