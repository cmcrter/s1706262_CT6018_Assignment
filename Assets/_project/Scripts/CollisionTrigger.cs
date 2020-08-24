using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            PlateTriggered();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isLocked)
        {
            PlateUntriggered();
        }
    }

    private void PlateTriggered()
    {
        if (TriggeredObjects.Count == 0) return;

        foreach (ITriggerable tObject in TriggeredObjects)
        {
            tObject.Triggered();
        }
    }

    private void PlateUntriggered()
    {
        if (TriggeredObjects.Count == 0) return;

        foreach (ITriggerable tObject in TriggeredObjects)
        {
            tObject.UnTriggered();          
        }
    }

    private void PlateLocked()
    {
        isLocked = true;
    }

    private void PlateUnlocked()
    {
        isLocked = false;
    }
}
