using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyCore : MonoBehaviour
{
    [Header("Energy")]
    float maxEnergy; // maximum amount of energy that can be stored by a core
    float currentEnergy;
    float baseEnergyRechargePerTick; // the amount of recharge that occurs every tick
    float baseEnergyTimeBetweenTicks; // the amount of time that ticks between recharge
    float baseMovementSpeed = 7f;

    [Header("Speed")]
    [Header("Power")]
    [Header("Recharge")]
    [Header("Shield")]
    float maxShield = 0.0f;
    float currentShield = 0.0f;
    float baseShieldRechargePerTick; // the amount of recharge that occurs every tick
    float baseShieldTimeBetweenTicks; // the amount of time that ticks between recharge
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
