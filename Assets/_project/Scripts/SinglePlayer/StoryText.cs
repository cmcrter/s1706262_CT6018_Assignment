////////////////////////////////////////////////////////////
// File: StoryText.cs
// Author: Charles Carter
// Brief: A class for the story text objects to show and hide at the right times
////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using TMPro;

public class StoryText : MonoBehaviour
{
    #region Class Variables

    [Header("Variables Needed")]
    [SerializeField]
    private TextMeshProUGUI thisText;
    //The waypoint manager loads and saves the current progress of the player
    //This includes what text they hit last
    [SerializeField]
    WaypointManager waypointManager;
    [SerializeField]
    private IEnumerator showingText;
    [SerializeField]
    private IEnumerator hidingText;
    [SerializeField]
    private float fFadeTimer;
    [SerializeField]
    private bool bAlreadyPlayed;
    [SerializeField]
    private bool bPlayOnStart;

    #endregion

    private void Awake()
    {
        thisText = thisText ?? GetComponent<TextMeshProUGUI>();
        showingText = showingText ?? Co_FadeInText(fFadeTimer);
        hidingText = hidingText ?? Co_FadeOutText(fFadeTimer);
    }

    // Start is called before the first frame update
    void Start()
    {
        //If there's no text to change
        if (!thisText)
        {
            if (Debug.isDebugBuild)
            {
                Debug.Log("Set a story text box for this trigger: " + name, this);
            }

            //There's not reason for this object to be enabled
            gameObject.SetActive(false);
        }

        //ToDo: Have it change text based on input handler

        //If it should start to show the text now
        if (bPlayOnStart && !bAlreadyPlayed)
        {
            //Show the text
            StartCoroutine(showingText);
        }
    }

    //This object also triggers the text showing or hiding
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //When the player enters the zone, and the text isnt shown
        if (collision.tag.Equals("Player") && !bAlreadyPlayed)
        {
            //Show the text
            StartCoroutine(showingText);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //When the player leaves the zone and the text is shown
        if (collision.tag.Equals("Player") && bAlreadyPlayed && enabled)
        {
            //Hide the text, even if it's showing the text already it still works out
            StartCoroutine(hidingText);
        }
    }

    //Functions for fading the text
    private IEnumerator Co_FadeInText(float fTimer)
    {
        bAlreadyPlayed = true;

        for (float t = 0; t < fTimer; t += Time.deltaTime)
        {
            //Changing the alpha based on the current time of the ienumerator 
            thisText.color = new Color(thisText.color.r, thisText.color.g, thisText.color.b, t / fTimer);
            yield return null;
        }
    }

    private IEnumerator Co_FadeOutText(float fTimer)
    {
        //This shouldn't be needed again
        if (showingText.MoveNext())
        {
            StopCoroutine(showingText);
        }

        for (float t = fTimer; t > 0; t -= Time.deltaTime)
        {
            //Changing the alpha based on the current time of the ienumerator 
            thisText.color = new Color(thisText.color.r, thisText.color.g, thisText.color.b, t / fTimer);
            yield return null;
        }

        //If there's a waypoint manager
        if (waypointManager)
        {
            //Tell it this text is done
            waypointManager.TextDone();
        }

        //This trigger wont be used again
        enabled = false;
    }
}
