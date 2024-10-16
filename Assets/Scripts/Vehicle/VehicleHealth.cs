using System.Collections;
using System.Collections.Generic;
using BT.Save;
using UnityEngine;
using UnityEngine.Serialization;

public class VehicleHealth : MonoBehaviour
{
    [SerializeField] int maxHealth;
    int currentHeath;

    [SerializeField] float healtRegenTime;

    [SerializeField] RSE_IntEvent OnTakeDamage;
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
            if(currentHeath <= maxHealth /2) OnTakeDamage.Call(currentHeath);
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
        // avalancheFollow.Bury();
        OnTakeDamage.Call(currentHeath);
        gameObject.SetActive(false);
    }

    IEnumerator Regen()
    {
        yield return new WaitForSeconds(healtRegenTime);
        TakeHealth(1);
    }
}
