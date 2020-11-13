using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is a trigger that needs the player to press a button to activate/unactivate
public class InputTrigger : InteractableTrigger
{
    [SerializeField]
    GameObject handle;

    private void Awake()
    {
        handle = handle ?? transform.GetChild(0).gameObject;
    }

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
    public void InputTriggered()
    {
        //guard clause
        if (isLocked) return;
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
        CheckTriggered();
    }
}
