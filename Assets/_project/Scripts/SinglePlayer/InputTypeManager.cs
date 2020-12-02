using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//Changing the input manager on the player (for singleplayer only)
public class InputTypeManager : MonoBehaviour
{
    [SerializeField]
    List<aHandlesInput> scriptsThatRequireInput = new List<aHandlesInput>();

    [SerializeField]
    KeyAndMouseHandler keyAndMouseHandler;
    [SerializeField]
    XboxControllerHandler xboxControllerHandler;

    bool controllerLastFrame = false;

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
