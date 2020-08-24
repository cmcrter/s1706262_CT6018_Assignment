using UnityEngine;
using UnityEngine.UI;

public class RadialTimerUI : MonoBehaviour
{
    [SerializeField]
    private Image radialImge;
    private float timerDuration;

    public void PrepUI(float maxDuration)
    {
        timerDuration = maxDuration;
    }

    public void TimerTick(float current)
    {
        // Calculate the current fill percentage and display it
        float fillPercentage = 1 - (current / timerDuration);

        if (fillPercentage > 0)
        {
            radialImge.fillAmount = fillPercentage;
        }
    }
}
