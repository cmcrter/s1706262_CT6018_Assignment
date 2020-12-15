using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersusMap : MonoBehaviour
{
    [SerializeField]
    private List<PuzzleChamber> mapChambers;

    //[SerializeField]
    //private WeaponSpawner spawner;

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
