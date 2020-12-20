////////////////////////////////////////////////////////////
// File: VersusMap.cs
// Author: Charles Carter
// Brief: A container class for the maps in the versus gamemode
////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersusMap : MonoBehaviour
{
    //The list of chambers
    [SerializeField]
    private List<PuzzleChamber> mapChambers;

    //The weapon spawners on the map
    //[SerializeField]
    //private List<WeaponSpawner> spawner;

    private void Awake()
    {
        if (!mapChambers[0])
        {
            if (Debug.isDebugBuild)
            {
                Debug.Log("Set puzzle chambers in the inspector");
            }

            enabled = false;
        }
    }

    public List<PuzzleChamber> GetChambers()
    {      
        return mapChambers;
    }
}
