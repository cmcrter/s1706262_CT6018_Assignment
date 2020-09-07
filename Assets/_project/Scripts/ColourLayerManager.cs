using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CState
{
    int ID;
    LayerMask mask;
}

public class ColourLayerManager : MonoBehaviour
{
    [SerializeField]
    Dictionary<Color, CState> colourPair = new Dictionary<Color, CState>();

    private void Awake()
    {
        
    }
}
