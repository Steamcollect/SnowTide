using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : Interactible
{
    [SerializeField] bool disableAfterHit = true;
    [SerializeField] int damage;

    public override void ResetComponent()
    {
        gameObject.SetActive(true);
    }

    public override void OnPlayerCollision(Transform player)
    {
        if (player.TryGetComponent(out VehicleHealth vehicleHealth))
        {
            vehicleHealth.TakeDamage(damage);
            if (disableAfterHit) gameObject.SetActive(false);
        }
    }
}