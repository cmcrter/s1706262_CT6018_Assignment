using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is a trigger which is activated by something entering
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
