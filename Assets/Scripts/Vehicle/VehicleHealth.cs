using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleHealth : MonoBehaviour
{
    [SerializeField] int maxHealth;
    int currentHeath;

    [SerializeField] float healtRegenTime;

    [SerializeField] RSE_EventBasic rse_onDeath;
    [SerializeField] AvalancheFollow avalancheFollow;

    Coroutine regenCoroutine;

    private void Start()
    {
        currentHeath = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHeath -= damage;
        if (regenCoroutine != null) StopCoroutine(regenCoroutine);

        if (currentHeath <= 0) Die();
        else
        {
            if(currentHeath <= maxHealth /2) avalancheFollow.Show();
            regenCoroutine = StartCoroutine(Regen());
        }
    }

    public void TakeHealth(int health)
    {
        currentHeath += health;
        if (currentHeath > maxHealth)
        {
            currentHeath = maxHealth;
        }
        else if (currentHeath < maxHealth)
        {
            regenCoroutine = StartCoroutine(Regen());
        }

        if(currentHeath >= maxHealth /2) avalancheFollow.Hide();
    }

    void Die()
    {
        avalancheFollow.Bury();
        rse_onDeath.Call();
        gameObject.SetActive(false);
    }

    IEnumerator Regen()
    {
        yield return new WaitForSeconds(healtRegenTime);
        TakeHealth(1);
    }
}
