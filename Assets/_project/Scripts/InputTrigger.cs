using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTrigger : MonoBehaviour
{
    [SerializeField]
    List<MonoBehaviour> TriggeredObjects = new List<MonoBehaviour>();
    [SerializeField]
    GameObject handle;

    [SerializeField]
    private bool isLocked = false;
    private bool isActivated = false;
    [SerializeField]
    private bool bTriggerLock = false;

    private void Awake()
    {
        handle = handle ?? transform.GetChild(0).gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player") && collision.TryGetComponent<TriggerTracker>(out var tracker))
        {
            tracker.Add(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player") && collision.TryGetComponent<TriggerTracker>(out var tracker))
        {
            tracker.Remove(this);
        }
    }

    //This is a trigger based on an input the player does within range
    public void InputTriggered()
    {
        //Bit of a double guard clause
        if (isLocked || TriggeredObjects.Count == 0) return;

        foreach (MonoBehaviour tObject in TriggeredObjects)
        {
            if (tObject.TryGetComponent<ITriggerable>(out var triggerable) && tObject.enabled)
            {
                if (isActivated)
                {
                    if (triggerable.GetLockState() && !bTriggerLock)
                    {
                        triggerable.Unlocked();
                    }

                    triggerable.UnTriggered();
                }
                else
                {
                    triggerable.Triggered();
                    triggerable.Locked();
                }
            }
        }

        if (handle)
        {
            if (isActivated)
            {
                handle.transform.Rotate(new Vector3(0, 0, 13));
            }
            else
            {
                handle.transform.Rotate(new Vector3(0, 0, -13));
            }
        }

        isActivated = !isActivated;

        if (bTriggerLock)
        {
            TriggerLocked();
        }
    }

    private void TriggerLocked()
    {
        isLocked = true;
    }

    private void TriggerUnLocked()
    {
        isLocked = false;
    }
}
