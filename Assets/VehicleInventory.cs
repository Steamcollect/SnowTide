using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleInventory : MonoBehaviour
{
    [SerializeField] int peopleCount;
    [SerializeField] float currentWeight;

    public void AddPeople(PeopleStatistics people)
    {
        peopleCount += people.peopleCount;
        currentWeight += people.wheight;
    }
}