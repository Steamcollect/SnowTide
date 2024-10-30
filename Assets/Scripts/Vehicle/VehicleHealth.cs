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


    private void Start()
    {
        rsoLife.Value = new HealthData{maxHealth = maxHealth, health = maxHealth};
    }

    public void TakeDamage(int damage)
    {
        rsoLife.Value = new HealthData{maxHealth = maxHealth, health = rsoLife.Value.health - damage};
        if (regenCoroutine != null) StopCoroutine(regenCoroutine);

        if (rsoLife.Value.health <= 0) Die();
        else
        {
            regenCoroutine = StartCoroutine(Regen());
        }
    }

    private void TakeHealth(int health)
    {
        rsoLife.Value = new HealthData{maxHealth = maxHealth, health = rsoLife.Value.health + health};
        if (rsoLife.Value.health > maxHealth)
        {
            rsoLife.Value = new HealthData{maxHealth = maxHealth, health = maxHealth};
        }
        else if (rsoLife.Value.health < maxHealth)
        {
            regenCoroutine = StartCoroutine(Regen());
        }

        // if(currentHeath >= maxHealth /2) avalancheFollow?.Hide();
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
}
