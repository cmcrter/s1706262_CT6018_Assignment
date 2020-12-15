using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This is a script for all of the panels a UI (so it can be coloured correctly)
public class UIPanel : MonoBehaviour
{
    [SerializeField]
    private ColourLayerManager colours;

    [SerializeField]
    private Image panelImage;

    [SerializeField]
    private int iPanelID = 0;

    //This just uses it's current ID
    [ContextMenu("Set Panel State")]
    public void SetPanel()
    {
        panelImage.color = colours.clStates[iPanelID].UIcolour;
    }

    public void OpenPanel(int ID)
    {
        iPanelID = ID;

        panelImage.color = colours.clStates[ID].UIcolour;
    }

    public int GetPanelID()
    {
        return iPanelID;
    }
}
