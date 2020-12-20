////////////////////////////////////////////////////////////
// File: WaypointManager.cs
// Author: Charles Carter
// Brief: The manager class that waypoints in the singleplayer
////////////////////////////////////////////////////////////

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

    #endregion

    private void TeleportPlayerToWaypoint()
    {
        Player.position = waypointObjects[iWaypointCount].position;
    }

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

    private void SaveWaypoint()
    {
        PlayerPrefs.SetString("Waypoints Set", "true");
        PlayerPrefs.SetInt("Last Waypoint", iWaypointCount);
    }

    private void LoadWaypoint()
    {
        if (PlayerPrefs.HasKey("Waypoints Set"))
        {
            iWaypointCount = PlayerPrefs.GetInt("Last Waypoint");
            TeleportPlayerToWaypoint();
        }
    }
}
