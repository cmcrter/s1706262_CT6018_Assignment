////////////////////////////////////////////////////////////
// File: RadialTimerUI.cs
// Author: Charles Carter
// Brief: A class to display a timer using a unfilling radial circle
////////////////////////////////////////////////////////////

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

        //If it's above 0
        if (fillPercentage > 0)
        {
            //have the image filled by that percentage
            radialImge.fillAmount = fillPercentage;
        }
    }
}
