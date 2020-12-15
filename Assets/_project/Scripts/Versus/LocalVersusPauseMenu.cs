using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalVersusPauseMenu : PauseMenu
{
    [SerializeField]
    LocalVersusManager versusManager;

    [SerializeField]
    List<InputHandler> inputHandlers;

    private bool bMenuLocked = false;
    private InputHandler handlerUsed;

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
}
