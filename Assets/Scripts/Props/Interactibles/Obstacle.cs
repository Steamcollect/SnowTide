using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : Interactible
{
    [SerializeField] int damage;

    public override void ResetComponent()
    {
        throw new System.NotImplementedException();
    }

    public override void OnPlayerCollision(Transform player)
    {
        if (player.TryGetComponent(out VehicleHealth vehicleHealth))
        {
            vehicleHealth.TakeDamage(damage);
            gameObject.SetActive(false);
        }
    }
}