﻿using UnityEngine;
using UnityEngine.UI;

public class AntiAcidBoots : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    [SerializeField] private float boostDuration = 5f;
    [SerializeField] private float speedBoost = 2f;
    [SerializeField] private Image chargeBar;
    [SerializeField] private AudioSource boostSound;
    [SerializeField] private GameObject dustEffect;

    public bool IsBoostActive { get; private set; } = false;
    private float currentCharge = 1f;

    private void Update()
    {
        if (IsBoostActive)
        {
            currentCharge -= Time.deltaTime / boostDuration;
            chargeBar.fillAmount = currentCharge;

            if (currentCharge <= 0f)
            {
                DeactivateBoost();
            }
        }
    }

    public void ActivateBoost()
    {
        if (currentCharge > 0f && !IsBoostActive)
        {
            IsBoostActive = true;
            playerController.AddSpeedModifier(speedBoost);

            if (boostSound != null)
                boostSound.Play();

            if (dustEffect != null)
                dustEffect.SetActive(true);
        }
    }

    public void DeactivateBoost()
    {
        if (IsBoostActive)
        {
            IsBoostActive = false;
            playerController.RemoveSpeedModifier(speedBoost);

            if (dustEffect != null)
                dustEffect.SetActive(false);

            if (boostSound != null && boostSound.isPlaying)
                boostSound.Stop();
        }
    }

    public void ResetBoostCharge()
    {
        currentCharge = 1f;
        chargeBar.fillAmount = currentCharge;
    }
}
