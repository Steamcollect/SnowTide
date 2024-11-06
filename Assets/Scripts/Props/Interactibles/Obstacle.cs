using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : Interactible
{
    [SerializeField] int damage;

    public override void ResetComponent()
    {
        gameObject.SetActive(true);
    }

    public override void OnPlayerCollision(Transform player)
    {
        print(transform.parent.parent.name);
        if (player.TryGetComponent(out VehicleHealth vehicleHealth))
        {
            vehicleHealth.TakeDamage(damage);
            gameObject.SetActive(false);
        }
    }
}