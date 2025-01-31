﻿////////////////////////////////////////////////////////////
// File: DamageTrigger.cs
// Author: Charles Carter
// Brief: An interactable trigger which activates/unactivates based on taking damage or healing
////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inherits from interactable trigger
public class DamageTrigger : InteractableTrigger, IDamagable
{
    #region Interface Contracts

    void IDamagable.Damage(float amount) => DamagedTrigger(amount);
    void IDamagable.Heal(float amount) => HealedTrigger(amount);

    #endregion

    [Header("Damage Trigger Specific Variables")]
    [SerializeField]
    private float fTriggerHealth;
    [SerializeField]
    private float fMaxTriggerHealh;

    private void Start()
    {
        if (fMaxTriggerHealh == 0 && Debug.isDebugBuild)
        {
            Debug.Log("This damage trigger has no health", this);
        }

        fTriggerHealth = fMaxTriggerHealh;
    }

    //Trigger was damaged or healed
    private void DamagedTrigger(float damageAmount)
    {
        fTriggerHealth -= damageAmount;
        CheckIfTriggered();
    }

    private void HealedTrigger(float healAmount)
    {
        fTriggerHealth += healAmount;

        if (fTriggerHealth > fMaxTriggerHealh)
        {
            fTriggerHealth = fMaxTriggerHealh;
        }
    }

    //If the health has dropped to 0 or below 0, trigger whatever it was supposed to
    private void CheckIfTriggered()
    {
        if (fTriggerHealth <= 0)
        {
            fTriggerHealth = 0;

            CheckTriggered();
        }
    }
}
