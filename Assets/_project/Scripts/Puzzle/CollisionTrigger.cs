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
            TriggerActivated();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isLocked)
        {
            TriggerUnactivated();
        }
    }

    private void TriggerActivated()
    {
        if (TriggeredObjects.Count == 0) return;

        foreach (ITriggerable tObject in TriggeredObjects)
        {
            tObject.Triggered();
        }
    }

    private void TriggerUnactivated()
    {
        if (TriggeredObjects.Count == 0) return;

        foreach (ITriggerable tObject in TriggeredObjects)
        {
            tObject.UnTriggered();          
        }
    }
}
