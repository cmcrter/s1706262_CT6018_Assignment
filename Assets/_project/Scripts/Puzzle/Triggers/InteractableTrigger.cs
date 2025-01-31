﻿////////////////////////////////////////////////////////////
// File: InteractableTrigger.cs
// Author: Charles Carter
// Brief: The base class which has the functionality all triggers will need
////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableTrigger : MonoBehaviour
{
    [SerializeField]
    List<InteractableManager> interactableManagers;
    //All triggers can get locked and unlocked
    [SerializeField]
    public bool isLocked;
    //For timers and levers
    public bool isActivated = false;

    //This trigger was set off
    protected void CheckTriggered()
    {
        foreach (InteractableManager manager in interactableManagers)
        {
            manager.CheckTriggerStates();
        }
    }

    public virtual void UnLockTrigger()
    {
        isLocked = false;
    }

    public virtual void LockTrigger()
    {
        isLocked = true;
    }
}
