using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerTrigger : MonoBehaviour
{
    [SerializeField]
    List<MonoBehaviour> triggerables = new List<MonoBehaviour>();

    [SerializeField]
    private bool isLocked = false;
    private bool isActive = false;

    [SerializeField]
    private bool isOverrideTimer = false;
    [SerializeField]
    private float timerDuration;

    [SerializeField]
    RadialTimerUI timerUI;

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
        isActive = true;
        float t = 0;

        timerUI.PrepUI(duration);

        while (isActive)
        {
            if (!isLocked)
            {
                t += Time.deltaTime;
                timerUI.TimerTick(t);

                if (t >= duration)
                {
                    isActive = false;
                }
            }

            yield return null;
        }

        ActivateTriggerables();
    }

    private void ActivateTriggerables()
    {
        if (triggerables.Count == 0) return;

        foreach (MonoBehaviour tComponent in triggerables)
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
