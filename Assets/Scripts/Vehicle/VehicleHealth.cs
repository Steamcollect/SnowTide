using System.Collections;
using System.Collections.Generic;
using BT.Save;
using DG.Tweening;
using UnityEngine;

public class VehicleHealth : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private int maxHealth;
    [SerializeField] private float healtRegenTime;

    [Space(5)]
    [SerializeField] float invincibilityDelay;
    [HideInInspector] public bool isInvincible = false;
    [SerializeField] float invincibilityFlahDelay;

    private Coroutine regenCoroutine;

    [Header("References")]
    [SerializeField] GameObject graphics;
    [Space(5)]
    [SerializeField] private RSO_Life rsoLife;
    [SerializeField] private RSE_Event OnPlayerDeath;
    [SerializeField] RSO_Camera rso_Cam;
    [SerializeField] RSO_TakeDamageCrack rsoTakeDamageCrack;
    [Space(5)]
    [SerializeField] RSE_ToggleSpeedLines rseToggleSpeedLines;
    [SerializeField] GameObject onDeathParticle;

    public void Start()
    {
        rsoLife.Value = new HealthData{maxHealth = maxHealth, health = maxHealth, isRegen = true};
    }

    public void TakeDamage(int damage)
    {
        print("take damage");
        if (isInvincible) return;

        if (regenCoroutine != null) StopCoroutine(regenCoroutine);
        rsoLife.Value = new HealthData{maxHealth = maxHealth, health = rsoLife.Value.health - damage};
        if (rsoLife.Value.health <= 0) Die();
        else
        {
            rseToggleSpeedLines.Call(false);

            transform.BumpVisual();
            rso_Cam.Value.BumpFieldOfView();

            for (int i = 0; i < rsoTakeDamageCrack.Value.Length; i++)
            {
                rsoTakeDamageCrack.Value[i].DOKill();
                rsoTakeDamageCrack.Value[i].DOFade(.7f, .04f);
            }

            regenCoroutine = StartCoroutine(Regen());
            StartCoroutine(InvincibilityDelay());
        }
    }

    private void TakeHealth(int health)
    {
        rsoLife.Value = new HealthData
        {
            maxHealth = maxHealth, 
            health = Mathf.Clamp(0,rsoLife.Value.maxHealth,rsoLife.Value.health + health), 
            isRegen = true
        };

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
        Instantiate(onDeathParticle, transform.position, Quaternion.identity);
        OnPlayerDeath.Call();
    }

    IEnumerator Regen()
    {
        yield return new WaitForSeconds(healtRegenTime);
        TakeHealth(1);
        for (int i = 0; i < rsoTakeDamageCrack.Value.Length; i++)
        {
            rsoTakeDamageCrack.Value[i].DOKill();
            rsoTakeDamageCrack.Value[i].DOFade(0, 1f);
        }
        rseToggleSpeedLines.Call(true);
    }
}

[System.Serializable]
public struct HealthData
{
    public int health;
    public int maxHealth;
    public bool isRegen;
}
