using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocalVersusPauseMenu : PauseMenu
{
    [SerializeField]
    LocalVersusManager versusManager;

    [SerializeField]
    List<InputHandler> inputHandlers;

    private bool bMenuLocked = false;
    private InputHandler handlerUsed;

    //Button when a player wants to quit
    [SerializeField]
    GameObject quitButton;
    //Button when the 1st player wants to go back to main menu
    [SerializeField]
    GameObject exitButton;

    public void AddHandler(InputHandler newHandler)
    {
        inputHandlers.Add(newHandler);
    }

    //The versus gamemode can have different input handlers currently playing
    public void SetHandlers(List<InputHandler> newHandlers)
    {
        inputHandlers = newHandlers;
    }

    // Update is called once per frame
    void Update()
    {
        //If another player doesnt the menu open
        if (!bMenuLocked)
        {
            //Checking if any of the active players press the menu button
            foreach (InputHandler handler in inputHandlers)
            {
                if (handler.ToggleMenu().Item1)
                {
                    handlerUsed = handler;

                    ToggleMenu(handler.ToggleMenu().Item2);

                    //Player 1 pressed the menu
                    if (handler == inputHandlers[0])
                    {
                        quitButton.SetActive(false);
                        exitButton.SetActive(true);
                    }

                    bMenuLocked = true;
                }
            }
        }
        //If they do
        else
        {
            if (handlerUsed.ToggleMenu().Item1)
            {
                ToggleMenu(handlerUsed.ToggleMenu().Item2);

                handlerUsed = null;
                bMenuLocked = false;
            }
        }
    }

    public void ExitGamemode()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
