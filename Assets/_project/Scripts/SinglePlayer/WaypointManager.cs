using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour, ISaveable
{
    #region Interface Contracts

    void ISaveable.Save() => SaveWaypoint();
    void ISaveable.Load() => LoadWaypoint();

    #endregion

    [Header("Variables Needed")]
    //To keep track of where the player last was
    [SerializeField]
    Transform Player;
    [SerializeField]
    Transform[] waypointObjects;
    Waypoint latestWaypoint;
    int iWaypointCount = 0;

    private void TeleportPlayerToWaypoint()
    {
        Player.position = waypointObjects[iWaypointCount].position;
    }

    public void LogWaypoint(Waypoint thisWaypoint)
    {
        latestWaypoint = thisWaypoint;
        iWaypointCount++;

        if (Debug.isDebugBuild)
        {
            Debug.Log("Waypoint Hit");
        }
    }

    void SaveWaypoint()
    {
        PlayerPrefs.SetString("Waypoints Set", "true");
        PlayerPrefs.SetInt("Last Waypoint", iWaypointCount);
    }

    void LoadWaypoint()
    {
        if (PlayerPrefs.HasKey("Waypoints Set"))
        {
            iWaypointCount = PlayerPrefs.GetInt("Last Waypoint");
            TeleportPlayerToWaypoint();
        }
    }
}
