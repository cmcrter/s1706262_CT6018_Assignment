using UnityEngine;
using UnityEngine.SceneManagement;

public class SingeplayerPauseMenu : PauseMenu
{
    [Header("Singleplayer Specific Variables")]
    [SerializeField]
    SaveManager saveManager;
    [SerializeField]
    CState playerState;

    private void Update()
    {
        if (inputHandler.ToggleMenu().Item1)
        {
            ToggleMenu(playerState.returnID());
        }
    }

    public void SaveButton()
    {
        saveManager.SaveAllSaveables();
    }

    public void LoadButton()
    {
        saveManager.LoadAllSaveables();
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
