////////////////////////////////////////////////////////////
// File: InputTypeManager.cs
// Author: Charles Carter
// Brief: The manager class that handles switches the input type (Keyboard, Gamepad) in singleplayer
////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//Changing the input manager on the player (for singleplayer only)
public class InputTypeManager : MonoBehaviour
{
    #region Class Variables

    [Header("Variables Needed")]
    [SerializeField]
    private List<aHandlesInput> scriptsThatRequireInput = new List<aHandlesInput>();
    [SerializeField]
    private KeyAndMouseHandler keyAndMouseHandler;
    [SerializeField]
    private XboxControllerHandler xboxControllerHandler;
    private bool controllerLastFrame = false;

    #endregion

    // Update is called once per frame
    void Update()
    {
        DevicesCheck();
    }

    private void DevicesCheck()
    {
        if (xboxControllerHandler.isBeingUsed() && !controllerLastFrame)
        {
            controllerLastFrame = true;
            ChangeAllInputHandlers(controllerLastFrame);
        }       
        else if (keyAndMouseHandler.isBeingUsed() && controllerLastFrame)
        {
            controllerLastFrame = false;
            ChangeAllInputHandlers(controllerLastFrame);
        }
    }

    private void ChangeAllInputHandlers(bool controller)
    {
        foreach (aHandlesInput script in scriptsThatRequireInput)
        {
            if (controller)
            {
                script.SetInputHandler(xboxControllerHandler);
            }
            else
            {
                script.SetInputHandler(keyAndMouseHandler);
            }
        }
    }
}
