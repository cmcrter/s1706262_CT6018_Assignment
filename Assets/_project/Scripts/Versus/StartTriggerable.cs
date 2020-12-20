////////////////////////////////////////////////////////////
// File: StartTriggerable.cs
// Author: Charles Carter
// Brief: Projectiles that bounce off of the surfaces
////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//This is a script specifically for the lever in the local versus lobby
public class StartTriggerable : MonoBehaviour, ITriggerable
{
    #region Interface Contracts

    void ITriggerable.Triggered() => StartCountdown();
    void ITriggerable.UnTriggered() => StopCountdown();
    void ITriggerable.Locked() => Lock();
    void ITriggerable.Unlocked() => UnLock();
    bool ITriggerable.GetLockState() => isLocked();

    #endregion
    
    #region Class Variables

    [Header("Variables Needed")]
    [SerializeField]
    private LocalVersusManager versusManager;
    private IEnumerator Co_GameCooldown;
    [SerializeField]
    private TextMeshProUGUI coolDownText;
    //Timer can be in whole seconds for the countdown
    [SerializeField]
    private int iCountdownTimer;
    private float currentT = 0;
    private bool bStartedCountdownBefore = false;

    #endregion

    private void Awake()
    {
        versusManager = versusManager ?? GameObject.FindGameObjectWithTag("VersusManager").GetComponent<LocalVersusManager>();
        Co_GameCooldown = Co_MatchCooldown(iCountdownTimer);
    }

    private IEnumerator Co_MatchCooldown(int iTimer)
    {
        //The cooldown for the lobby lever
        for (currentT = 0; currentT < iTimer; currentT += Time.deltaTime)
        {
            //Manging the lever based on versus variables and updating the countdown
            if (bStartedCountdownBefore)
            {
                coolDownText.text = iTimer.ToString();
                bStartedCountdownBefore = false;
            }

            if (versusManager.GetActivePlayerCount() < 2)
            {
                StopCountdown();
            }

            //Every Second
            if (currentT % 1 < Time.deltaTime)
            {
                //Update the text to say how long is left
                coolDownText.text = (iTimer - ((int)currentT)).ToString();
            }

            yield return null;
        }

        versusManager.StartLeverPulled();
    }

    private void StartCountdown()
    {
        if (versusManager.GetActivePlayerCount() > 1)
        {
            bStartedCountdownBefore = true;
            StartCoroutine(Co_GameCooldown);
        }
    }

    private void StopCountdown()
    {
        StopCoroutine(Co_GameCooldown);

        currentT = 0;

        //Resetting the text
        coolDownText.text = "Start";
    }

    private void Lock(){ }
    private void UnLock(){ }
    private bool isLocked()
    {
        return false;
    }
}
