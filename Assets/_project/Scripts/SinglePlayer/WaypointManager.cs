////////////////////////////////////////////////////////////
// File: WaypointManager.cs
// Author: Charles Carter
// Brief: The manager class that waypoints in the singleplayer
////////////////////////////////////////////////////////////

using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour, ISaveable
{
    #region Interface Contracts

    void ISaveable.Save() => SaveWaypoint();
    void ISaveable.Load() => LoadWaypoint();

    #endregion

    #region Variables

    [Header("Variables Needed")]
    //To keep track of where the player last was
    [SerializeField]
    private Transform Player;
    [SerializeField]
    private Transform[] waypointObjects;
    private Waypoint latestWaypoint;
    private int iWaypointCount = 0;
    [SerializeField]
    private StoryText[] NarrativeTextTriggers;
    int iTextsCount = 0;

    #endregion

    #region Public Class Functions

    //Recording the hit waypoint
    public void LogWaypoint(Waypoint thisWaypoint)
    {
        latestWaypoint = thisWaypoint;
        iWaypointCount++;

        if (Debug.isDebugBuild)
        {
            Debug.Log("Waypoint Hit");
        }
    }

    //The player has left a text area
    public void TextDone()
    {
        iTextsCount++;
    }

    #endregion

    #region Private Class Functions

    //Teleporting the player to the previous waypoint
    private void TeleportPlayerToWaypoint()
    {
        Player.position = waypointObjects[iWaypointCount].position;
    }

    //Turning off the previous texts done
    private void TurningOffFinishedTexts(int iLastText)
    {
        //Going through the last texts to make sure they dont play again
        for (int i = 0; i < iLastText; ++i)
        {
            //Eventually this wont be needed, it would be a function to lock them
            NarrativeTextTriggers[i].gameObject.SetActive(false);
        }
    }

    private void SaveWaypoint()
    {
        //Saving the relevant details with the right keys
        PlayerPrefs.SetString("Waypoints Set", "true");
        PlayerPrefs.SetInt("Last Waypoint", iWaypointCount);
        PlayerPrefs.SetInt("Last Text", iTextsCount);
    }

    private void LoadWaypoint()
    {
        //If it has waypoint details to load
        if (PlayerPrefs.HasKey("Waypoints Set"))
        {
            //Loading the details from the right keys
            iWaypointCount = PlayerPrefs.GetInt("Last Waypoint");
            iTextsCount = PlayerPrefs.GetInt("Last Text");
            TeleportPlayerToWaypoint();
            TurningOffFinishedTexts(iTextsCount);
        }
    }

    #endregion
}
