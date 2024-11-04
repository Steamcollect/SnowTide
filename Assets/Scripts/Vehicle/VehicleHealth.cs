using System.Collections;
using System.Collections.Generic;
using BT.Save;
using UnityEngine;

public class VehicleHealth : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private int maxHealth;
    [SerializeField] private float healtRegenTime;

    [Space(5)]
    [SerializeField] float invincibilityDelay;
    bool isInvincible = false;
    [SerializeField] float invincibilityFlahDelay;

    private Coroutine regenCoroutine;

    [Header("References")]
    [SerializeField] GameObject graphics;
    [Space(5)]
    [SerializeField] private RSO_Life rsoLife;
    [SerializeField] private RSE_Event OnPlayerDeath;
    [SerializeField] private AvalancheFollow avalancheFollow;
    
    public void Start()
    {
        rsoLife.Value = new HealthData{maxHealth = maxHealth, health = maxHealth, isRegen = true};
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        if (regenCoroutine != null) StopCoroutine(regenCoroutine);
        rsoLife.Value = new HealthData{maxHealth = maxHealth, health = rsoLife.Value.health - damage};
        if (rsoLife.Value.health <= 0) Die();
        else
        {
            regenCoroutine = StartCoroutine(Regen());
            StartCoroutine(InvincibilityDelay());
        }
    }

    private void TakeHealth(int health)
    {
        rsoLife.Value = new HealthData{maxHealth = maxHealth, health = Mathf.Clamp(0,rsoLife.Value.maxHealth,rsoLife.Value.health + health), isRegen = true};
        if (rsoLife.Value.health < maxHealth)
        {
            regenCoroutine = StartCoroutine(Regen());
        }
    }

    IEnumerator InvincibilityDelay()
    {
        StartCoroutine(InvincibilityFlash());

        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDelay);
        isInvincible = false;
    }
    IEnumerator InvincibilityFlash()
    {
        graphics.SetActive(false);
        yield return new WaitForSeconds(invincibilityFlahDelay);
        graphics.SetActive(true);
        yield return new WaitForSeconds(invincibilityFlahDelay);

        if(isInvincible) StartCoroutine(InvincibilityFlash());
    }

    private void Die()
    {
        OnPlayerDeath.Call();
    }

    IEnumerator Regen()
    {
        yield return new WaitForSeconds(healtRegenTime);
        TakeHealth(1);
    }
}

[System.Serializable]
public struct HealthData
{
    public int health;
    public int maxHealth;
    public bool isRegen;
}
