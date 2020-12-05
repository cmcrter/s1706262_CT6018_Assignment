using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CState : MonoBehaviour
{
    [SerializeField]
    ColourLayerManager manager;

    [SerializeField]
    int stateID;

    [SerializeField]
    List<Renderer> renderers = new List<Renderer>();

    [SerializeField]
    List<GameObject> layerObjects = new List<GameObject>();

    [SerializeField]
    bool isPlayer;

    public void SetState(int id, bool changeLayer)
    {
        stateID = id;

        if (changeLayer)
        {
            ChangeLayer();
        }
        ShowState();
    }



    [ContextMenu("Set State")]
    public void SetState()
    {
        ChangeLayer();
        ShowState();
    }

    private void ShowState()
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.sharedMaterial = manager.GetState(stateID).defaultMaterial;
        }
    }

    private void ChangeLayer()
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

    public int returnID()
    {
        return stateID;
    }

    public Color GetColor()
    {
        return manager.GetState(stateID).defaultMaterial.color;
    }

    public Material GetMaterial()
    {
        return manager.GetState(stateID).defaultMaterial;
    }
}
