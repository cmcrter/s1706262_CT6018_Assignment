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

    private void Start()
    {
        StartTimer();
    }

    public void StartTimer()
    {
        if (timerDuration < float.Epsilon)
        {
            if (Debug.isDebugBuild)
            {
                Debug.Log("Set a timer length in inspector", this);
            }

            timerDuration = 5f;
        }

        StartCoroutine(Co_TimerRun(timerDuration));
    }

    private IEnumerator Co_TimerRun(float duration)
    {
        isActivated = true;
        float t = 0;

        timerUI.PrepUI(duration);

        while (isActivated)
        {
            if (!isLocked)
            {
                t += Time.deltaTime;
                timerUI.TimerTick(t);

                if (t >= duration)
                {
                    isActivated = false;
                }
            }

            yield return null;
        }

        ActivateTriggerables();
    }

    private void ActivateTriggerables()
    {
        if (TriggeredObjects.Count == 0) return;

        foreach (MonoBehaviour tComponent in TriggeredObjects)
        {
            if (tComponent.TryGetComponent<ITriggerable>(out var triggerable))
            {
                triggerable.Triggered();
                triggerable.Locked();

                if (isOverrideTimer)
                {
                    tComponent.enabled = false;
                }
            }
        }
    }
}
