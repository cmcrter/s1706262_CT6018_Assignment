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
    int iTextsCount = 0;
    [SerializeField]
    BackgroundManager backgroundManager;

    #endregion

    #region Public Class Functions

    //Recording the hit waypoint
    public void LogWaypoint(Waypoint thisWaypoint)
    {
        latestWaypoint = thisWaypoint;
        iWaypointCount++;

        if (Debug.isDebugBuild)
        {
            Debug.Log("Waypoint Hit", this);
        }
    }

    //The player has left a text area
    public void TextDone()
    {
        iTextsCount++;
    }

    public Vector3 WaypointPos(int wayIndex)
    {
        return waypointObjects[wayIndex].position;
    }

    //Might need to start this waypoints associated background
    public void StartBackground()
    {
        int temp = GetWaypointBackgroundIndex();

        if (temp > 0)
        {
            backgroundManager.TurnAllBackgroundsOff();
        }

        backgroundManager.SetParallaxOffset(temp);
        backgroundManager.SetBackgroundActive(temp, true);
    }

    #endregion

    #region Private Class Functions

    private int GetWaypointBackgroundIndex()
    {
        return waypointObjects[iWaypointCount].GetComponent<Waypoint>().GetBackgroundIndex();
    }

    //Teleporting the player to the previous waypoint
    private void TeleportPlayerToWaypoint()
    {
        Player.position = WaypointPos(iWaypointCount);
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
            StartBackground();
        }
    }

    #endregion
}
