using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 50;
    private int currentHealth;
    private bool isDead;
    public delegate void HealthChanged(int currentHealth, int maxHealth);
    public event HealthChanged OnHealthChanged;
    public Action OnDie;
    [SerializeField] EndGamePanel endGamePanel;
    public List<DamageTypes> ImmuneDamageTypes { get; set; } = new List<DamageTypes>();
    private void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

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

    private void Die()
    {
        Debug.Log("Player has died!");

        if (TryGetComponent(out PlayerController playerController))
        {
            isDead = true;
            OnDie?.Invoke();
            playerController.SetState(new DeadState());
            endGamePanel.Show();
        }
    }
}
