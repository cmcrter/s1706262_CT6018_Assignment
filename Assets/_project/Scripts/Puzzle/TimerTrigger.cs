using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is a trigger set on a timer
public class TimerTrigger : InteractableTrigger
{
    [SerializeField]
    private bool isOverrideTimer = false;
    [SerializeField]
    private float timerDuration;

    [SerializeField]
    private RadialTimerUI timerUI;
    private IEnumerator timer;

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
