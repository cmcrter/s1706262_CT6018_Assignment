using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//A class to contain all of the buttons for the main menu
public class MainMenu : MonoBehaviour
{
    [SerializeField]
    Button continueButton;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("CStates" + name + ":") && continueButton)
        {
            continueButton.interactable = true;
        }
    }

    public void NewGame()
    {
        //This will do for now, but might want to set up a list of keys to delete instead
        PlayerPrefs.DeleteAll();

        ContinueGame();
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("TestingScene");
    }

    public void LoadLocalVersus()
    {
        SceneManager.LoadScene("LocalVersus");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
