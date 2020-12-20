////////////////////////////////////////////////////////////
// File: TimerTrigger.cs
// Author: Charles Carter
// Brief: A trigger that uses a timer to activate an interactable
////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is a trigger set on a timer, Inherits from the overal interactable trigger functionality
public class TimerTrigger : InteractableTrigger
{
    #region Class Variables

    [Header("Variables Needed")]
    [SerializeField]
    private bool isOverrideTimer = false;
    [SerializeField]
    private float timerDuration;
    [SerializeField]
    private RadialTimerUI timerUI;
    private IEnumerator timer;

    #endregion

    private void Start()
    {
        StartTimer();
    }

    public void StartTimer()
    {
        if (timerDuration == 0)
        {
            if (Debug.isDebugBuild)
            {
                Debug.Log("Set a timer length in inspector", this);
            }

            timerDuration = 5f;
        }

        timer = Co_TimerRun(timerDuration);
        StartCoroutine(timer);
    }

    private IEnumerator Co_TimerRun(float duration)
    {
        isActivated = false;
        float t = 0;

        timerUI.PrepUI(duration);

        while (!isActivated)
        {
            if (!isLocked)
            {
                t += Time.deltaTime;
                timerUI.TimerTick(t);

                if (t >= duration)
                {
                    isActivated = true;
                    CheckTriggered();
                    StopCoroutine(timer);
                }
            }

            yield return null;
        }
    }
}
