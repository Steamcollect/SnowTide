using System.Collections;
using System.Collections.Generic;
using BT.Save;
using UnityEngine;
using UnityEngine.Serialization;

public class VehicleHealth : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private int maxHealth;
    [SerializeField] private float healtRegenTime;
    
    private Coroutine regenCoroutine;

    [Header("References")]
    [SerializeField] private RSO_Life rsoLife;
    [SerializeField] private RSE_Event OnPlayerDeath;
    [SerializeField] private AvalancheFollow avalancheFollow;
    
    public void Start()
    {
        rsoLife.Value = new HealthData{maxHealth = maxHealth, health = maxHealth, isRegen = true};
    }

    public void TakeDamage(int damage)
    {
        if (regenCoroutine != null) StopCoroutine(regenCoroutine);
        rsoLife.Value = new HealthData{maxHealth = maxHealth, health = rsoLife.Value.health - damage};
        if (rsoLife.Value.health <= 0) Die();
        else
        {
            regenCoroutine = StartCoroutine(Regen());
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
