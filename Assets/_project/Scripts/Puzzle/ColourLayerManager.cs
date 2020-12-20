////////////////////////////////////////////////////////////
// File: ColourLayerManager.cs
// Author: Charles Carter
// Brief: The manager class that delegates the colours used in the scene
////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The state the object is in
[System.Serializable]
public class CLState
{
    public int ID;
    public Material playerMaterial;
    public Material worldMaterial;
    public Color UIcolour;
}

//A class to store the games' base player/object states
public class ColourLayerManager : MonoBehaviour
{
    //These are the 4 layers/colours of the game
    [SerializeField]
    public List<CLState> clStates = new List<CLState>();
    private CLState[] states = new CLState[4];

    private void Awake()
    {
        SetStatesArray();
    }

    public void SetStatesArray()
    {
        states = clStates.ToArray();
    }

    //Anything can get one of these states
    public CLState GetState(int ID)
    {
        return states[ID];
    }
}
