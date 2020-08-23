using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTrigger : MonoBehaviour
{
    [SerializeField]
    List<GameObject> TriggeredObjects = new List<GameObject>();

    [SerializeField]
    bool isLocked = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player") && !isLocked)
        {
            PlateTriggered();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player") && !isLocked)
        {
            PlateUntriggered();
        }
    }

    private void PlateTriggered()
    {
        if (TriggeredObjects.Count == 0) return;

        foreach (GameObject tObject in TriggeredObjects)
        {           
            if (tObject.TryGetComponent<ITriggerable>(out var triggered))
            {
                triggered.Triggered();
            }
        }
    }

    private void PlateUntriggered()
    {
        if (TriggeredObjects.Count == 0) return;

        foreach (GameObject tObject in TriggeredObjects)
        {
            if (tObject.TryGetComponent<ITriggerable>(out var triggered))
            {
                triggered.UnTriggered();
            }
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
