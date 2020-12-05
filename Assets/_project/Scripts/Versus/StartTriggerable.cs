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

    [Header("Variables Needed")]

    [SerializeField]
    private LocalVersusManager versusManager;

    private IEnumerator gameCooldown;

    [SerializeField]
    TextMeshProUGUI coolDownText;

    //Timer can be in whole seconds for the countdown
    [SerializeField]
    int iCountdownTimer;

    float currentT = 0;
    bool bStartedCountdownBefore = false;
    
    void Awake()
    {
        versusManager = versusManager ?? GameObject.FindGameObjectWithTag("VersusManager").GetComponent<LocalVersusManager>();
        gameCooldown = eMatchCooldown(iCountdownTimer);
    }

    private IEnumerator eMatchCooldown(int iTimer)
    {
        for (currentT = 0; currentT < iTimer; currentT += Time.deltaTime)
        {
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
            StartCoroutine(gameCooldown);
        }
    }

    private void StopCountdown()
    {
        StopCoroutine(gameCooldown);

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
