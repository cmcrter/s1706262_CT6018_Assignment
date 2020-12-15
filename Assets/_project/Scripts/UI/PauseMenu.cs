using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A class to store all the base functionality a pause menu will have, which needs to register when an input wants it to open
public class PauseMenu : aHandlesInput
{
    [Header("General Pause Menu Variables")]
    [SerializeField]
    private List<UIPanel> panels = new List<UIPanel>();

    [SerializeField]
    protected GameObject startPanel;
    [SerializeField]
    protected GameObject currentPanel;
    [SerializeField]
    protected bool bMenuOpen;
    [SerializeField]
    protected int menuID;

    //Explicit Functions for opening and closing the menu
    public void OpenMenu(int playerID)
    {
        menuID = playerID;

        startPanel.SetActive(true);
        currentPanel = startPanel;

        foreach (UIPanel panel in panels)
        {
            panel.OpenPanel(playerID);
        }

        //Not the best implementation but it works 
        Time.timeScale = 0;

        bMenuOpen = true;
    }

    public void CloseMenu()
    {
        currentPanel.SetActive(false);
        //Not the best implementation but it works 
        Time.timeScale = 1;

        bMenuOpen = false;
    }

    //Also one for toggling the menu
    public void ToggleMenu(int iPlayerID)
    {
        if (bMenuOpen)
        {
            CloseMenu();
        }
        else
        {
            OpenMenu(iPlayerID);
        }
    }
}
