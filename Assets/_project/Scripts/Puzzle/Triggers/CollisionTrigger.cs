////////////////////////////////////////////////////////////
// File: CollisionTrigger.cs
// Author: Charles Carter
// Brief: An interactable trigger which using a collision entering to activate and collision leaving to unactivate
////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTrigger : InteractableTrigger
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        InputTriggered();       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        InputTriggered();       
    }

    public virtual void InputTriggered()
    {
        //guard clause
        if (isLocked) return;
        isActivated = !isActivated;
        CheckTriggered();
    }
}
