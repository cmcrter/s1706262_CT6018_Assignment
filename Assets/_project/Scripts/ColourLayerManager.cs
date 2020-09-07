using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Object types require IDs
public enum ObjectType
{
    NONE = 0,
    PLAYER = 1,
    TRIGGER = 2
}

[System.Serializable]
public class CLState
{
    public int ID;
    public Color color;
    public ObjectType type;
    public LayerMask layer;
}

//A class to store the games' base player/object states
public class ColourLayerManager : MonoBehaviour
{
    //These are the 4 layers/colours of the game
    [SerializeField]
    public List<CLState> cState = new List<CLState>();

    //Anything can get one of these states
    public CLState GetState(int ID, ObjectType type)
    {
        return cState.ToArray()[ID];
    }
}
