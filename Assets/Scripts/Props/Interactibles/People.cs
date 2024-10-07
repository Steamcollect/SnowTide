using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class People : Interactible
{
    [SerializeField] PeopleStatistics statistics;

    public override void OnPlayerCollision(Transform player)
    {
        if (player.TryGetComponent(out VehicleInventory vehicleInventory))
        {
            vehicleInventory.AddPeople(statistics);
            gameObject.SetActive(false);
        }
    }
}