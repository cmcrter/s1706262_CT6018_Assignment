////////////////////////////////////////////////////////////
// File: CState.cs
// Author: Charles Carter
// Brief: A class that manages the state of the object (both the colour and the layer)
////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CState : MonoBehaviour, ISaveable
{
    #region Interface Contracts

    void ISaveable.Save() => SaveState();
    void ISaveable.Load() => LoadState();

    #endregion

    #region Class Variables

    [Header("Variables")]
    [SerializeField]
    private ColourLayerManager manager;
    [SerializeField]
    private int stateID;
    [SerializeField]
    private List<Renderer> renderers = new List<Renderer>();
    [SerializeField]
    private List<GameObject> layerObjects = new List<GameObject>();
    [SerializeField]
    private bool isPlayer;
    [SerializeField]
    private bool isObject;
    [SerializeField]
    private bool isSaved;

    #endregion

    //Setting the state based on a new ID and whether it will change layer based on the new state
    public void SetState(int id, bool changeLayer)
    {
        stateID = id;

        if (changeLayer)
        {
            ChangeLayer();
        }
        ShowState();
    }

    //A function for the inspector
    [ContextMenu("Set State")]
    public void SetState()
    {
        //Making sure the states have the correct values
        manager.SetStatesArray();

        //Dont want to change the layer if it's a gameworld layer object
        if (isObject)
        {
            ChangeLayer();
        }

        ShowState();
    }

    //Changing the object to display the new state
    private void ShowState()
    {
        if (manager)
        {
            foreach (Renderer renderer in renderers)
            {
                renderer.material = GetMaterial();
            }
        }
        else
        {
            manager = GameObject.FindGameObjectWithTag("ColourStateManager").GetComponent<ColourLayerManager>();

            foreach (Renderer renderer in renderers)
            {
                renderer.material = GetMaterial();
            }

            if (Debug.isDebugBuild)
            {
                Debug.Log("Set colour layer manager in the inspector", this);
            }
        }
    }

    private void ChangeLayer()
    {
        //Changing the layer of an object
        if (isObject)
        {
            //The first object colour layer
            int iLayerOffset = 13;

            //Changing it to the player one if needed
            if (isPlayer)
            {
                iLayerOffset = 9;
            }

            foreach (GameObject gObject in layerObjects)
            {
                gObject.layer = stateID + iLayerOffset;
            }
        }
        //Changing the layer of the gameworld
        else
        {
            foreach (GameObject gObject in layerObjects)
            {
                gObject.layer = 8;
            }
        }
    }

    public int returnID()
    {
        return stateID;
    }

    public Color GetColor()
    {
        if (isPlayer)
        {
            return manager.GetState(stateID).playerMaterial.color;
        }

        return manager.GetState(stateID).worldMaterial.color;
    }

    public Material GetMaterial()
    {
        if (isPlayer)
        {
            return manager.GetState(stateID).playerMaterial;
        }

        return manager.GetState(stateID).worldMaterial;
    }

    //Quickly being able to set and get the manager this uses
    public void SetManager(ColourLayerManager newManager)
    {
        manager = newManager;
    }

    public ColourLayerManager GetManager()
    {
        return manager;
    }

    //Interface Functions
    private void SaveState()
    {
        //Making sure it can be loaded in
        PlayerPrefs.SetString("CStates" + name + ":", "Set");
        PlayerPrefs.SetInt("CStates_" + name + "_ID", stateID);
    }

    private void LoadState()
    {
        if (PlayerPrefs.HasKey("CStates" + name + ":"))
        {
            stateID = PlayerPrefs.GetInt("CStates_" + name + "_ID");

            SetState();
        }
    }
}
