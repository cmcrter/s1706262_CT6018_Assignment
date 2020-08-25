using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is a trigger which is activated by something entering
public class CollisionTrigger : MonoBehaviour
{
    [SerializeField]
    List<MonoBehaviour> TriggeredObjects = new List<MonoBehaviour>();

    [SerializeField]
    private bool isLocked = false;

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

    private void TriggerLock()
    {
        isLocked = true;
    }

    private void TriggerUnlocked()
    {
        isLocked = false;
    }
}
