////////////////////////////////////////////////////////////
// File: TriggerTracker.cs
// Author: Charles Carter
// Brief: The manager class that manages the usage of input triggers the player can access
////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is on the player to keep track of the input triggers in range and to activate the correct one
public class TriggerTracker : aHandlesInput
{
    [SerializeField]
    private List<InputTrigger> triggers = new List<InputTrigger>();

    // Update is called once per frame
    void Update()
    {
        if (triggers.Count > 0 && inputHandler.Interact())
        {
            ClosestTriggerCheck().InputTriggered();            
        }
    }

    //Getting the closest input trigger
    private InputTrigger ClosestTriggerCheck()
    {
        InputTrigger returnTrigger;

        if (triggers.Count == 1)
        {
            returnTrigger = triggers[0];
        }
        else
        {
            returnTrigger = triggers[0];
            float currentDist = Vector2.Distance(gameObject.transform.position, triggers[0].transform.position);

            for (int i = 1; i < triggers.Count; ++i)
            {
                float distToCheck = Vector2.Distance(gameObject.transform.position, triggers[i].transform.position);

                if (distToCheck < currentDist)
                {
                    currentDist = distToCheck;
                    returnTrigger = triggers[i];
                }
            }
        }

        return returnTrigger;
    }

    public void Add(InputTrigger inputTrigger)
    {
        //Need to check the colour of the input trigger vs the current colour of the player
        if (1 == 1)
        {
            triggers.Add(inputTrigger);
        }
    }

    public void Remove(InputTrigger inputTrigger)
    {
        if (1 == 1)
        {
            triggers.Remove(inputTrigger);
        }
    }
}
