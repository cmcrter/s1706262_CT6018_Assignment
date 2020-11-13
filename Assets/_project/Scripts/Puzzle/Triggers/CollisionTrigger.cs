using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is a trigger which is activated by something entering
public class CollisionTrigger : InteractableTrigger
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isLocked)
        {
            isActivated = true;
            CheckTriggered();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isLocked)
        {
            isActivated = false;
            CheckTriggered();
        }
    }
}
