using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableTrigger : MonoBehaviour
{
    //Objects that get triggered from the trigger
    [SerializeField]
    protected List<MonoBehaviour> TriggeredObjects = new List<MonoBehaviour>();

    //All triggers can get locked and unlocked
    [SerializeField]
    protected bool isLocked;

    //For timers and levers
    protected bool isActivated = false;

    //Explicit functions to lock or unlock the trigger
    protected void TriggerLocked()
    {
        isLocked = true;
    }

    protected void TriggerUnLocked()
    {
        isLocked = false;
    }
}
