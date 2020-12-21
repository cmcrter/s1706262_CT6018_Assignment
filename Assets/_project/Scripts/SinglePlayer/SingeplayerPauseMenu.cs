////////////////////////////////////////////////////////////
// File: SingleplayerPauseMenu.cs
// Author: Charles Carter
// Brief: The pause menu for singleplayer specific functionality
////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.SceneManagement;

//Inherits from PauseMenu
public class SingeplayerPauseMenu : PauseMenu
{
    [Header("Singleplayer Specific Variables")]
    [SerializeField]
    private SaveManager saveManager;
    [SerializeField]
    private CState playerState;

    private void Update()
    {
        if (inputHandler.ToggleMenu().Item1)
        {
            ToggleMenu(playerState.returnID());
        }
    }

    //Button functionality
    public void SaveButton()
    {
        saveManager.SaveAllSaveables();
    }

    public void LoadButton()
    {
        saveManager.LoadAllSaveables();
    }
}
