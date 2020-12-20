////////////////////////////////////////////////////////////
// File: InputTrigger.cs
// Author: Charles Carter
// Brief: The trigger class that uses player inputs to activate and unactivate 
////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is a trigger that needs the player to press a button to activate/unactivate
public class InputTrigger : InteractableTrigger
{
    //Uses the player's trigger tracker to see what should get activated (since there could be more than one input trigger within the player's collider)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<TriggerTracker>(out var tracker) && !isLocked)
        {
            tracker.Add(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<TriggerTracker>(out var tracker) && !isLocked)
        {
            tracker.Remove(this);
        }
    }

    //This is a trigger based on an input the player does within range
    public virtual void InputTriggered()
    {
        //guard clause
        if (isLocked) return;
        isActivated = !isActivated;
        CheckTriggered();
    }
}
