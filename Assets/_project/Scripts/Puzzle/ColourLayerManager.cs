using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CLState
{
    public int ID;
    public Material defaultMaterial;
}

//A class to store the games' base player/object states
public class ColourLayerManager : MonoBehaviour
{
    //These are the 4 layers/colours of the game
    [SerializeField]
    public List<CLState> cState = new List<CLState>();

    private CLState[] states = new CLState[4];

    private void Awake()
    {
        states = cState.ToArray();
    }

    //Anything can get one of these states
    public CLState GetState(int ID)
    {
        return states[ID];
    }
}
