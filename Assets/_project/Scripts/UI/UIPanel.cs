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

    public void OpenPanel(int ID)
    {
        iPanelID = ID;

        panelImage.color = colours.cState[ID].UIcolour;
    }

    public int GetPanelID()
    {
        return iPanelID;
    }
}
