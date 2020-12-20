////////////////////////////////////////////////////////////
// File: SaveManager.cs
// Author: Charles Carter
// Brief: The manager class that saves and loads needed objects' variables
////////////////////////////////////////////////////////////

using System.Linq;
using UnityEngine;

//This is the manager for saving and loading in game
public class SaveManager : MonoBehaviour
{
    private bool bSaveablesRetrived = false;
    private ISaveable[] saveables;

    private void Awake()
    {
        GetAllSaveables();
    }

    //This is particularly expensive
    private void GetAllSaveables()
    {
        saveables = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>().ToArray();
        bSaveablesRetrived = true;
    }

    //Going through the saveables and using their save or load function, whether they are currently active or not
    public void SaveAllSaveables()
    {
        if (!bSaveablesRetrived)
        {
            GetAllSaveables();
        }

        for (int i = 0; i < saveables.Length; ++i)
        {
            saveables[i].Save();
        }
    }

    public void LoadAllSaveables()
    {
        if (!bSaveablesRetrived)
        {
            GetAllSaveables();
        }

        for (int i = 0; i < saveables.Length; ++i)
        {
            saveables[i].Load();
        }
    }
}
