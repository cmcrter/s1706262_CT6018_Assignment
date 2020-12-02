using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    //To keep track of where the player last was
    Waypoint latestWaypoint;
    int iWaypointCount = 0;

    public void LogWaypoint(Waypoint thisWaypoint)
    {
        latestWaypoint = thisWaypoint;
        iWaypointCount++;

        if (Debug.isDebugBuild)
        {
            Debug.Log("Waypoint Hit");
        }
    }
}
