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
    
    private int currentHeath;
    private Coroutine regenCoroutine;

    [Header("References")]
    [SerializeField] private RSE_IntEvent OnTakeDamage;
    [SerializeField] private RSE_Event OnPlayerDeath;
    [SerializeField] private AvalancheFollow avalancheFollow;


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
            if(currentHeath <= maxHealth /2) OnTakeDamage.Call(currentHeath);
            regenCoroutine = StartCoroutine(Regen());
        }
    }

    private void TakeHealth(int health)
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

        if(currentHeath >= maxHealth /2) avalancheFollow?.Hide();
    }

    private void Die()
    {
        avalancheFollow?.Bury();
        OnPlayerDeath.Call();
        gameObject.SetActive(false);
    }

    IEnumerator Regen()
    {
        yield return new WaitForSeconds(healtRegenTime);
        TakeHealth(1);
    }
}
