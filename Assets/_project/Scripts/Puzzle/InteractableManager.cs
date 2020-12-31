////////////////////////////////////////////////////////////
// File: InteractableManager.cs
// Author: Charles Carter
// Brief: The manager class that handles an interection between triggers and an interactable
////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The struct for a state which actiavates the triggerable in question
[System.Serializable]
struct TriggerState
{
    //the triggers for this trigger state
    [SerializeField] List<InteractableTrigger> triggers;

    //Locks the triggerabled after done
    public bool bTriggerLock;

    public bool CheckIfTriggered()
    {
        //Go through all the triggers
        foreach (InteractableTrigger trig in triggers)
        {
            //If any triggers are false return false
            if (!trig.isActivated)
            {
                return false;
            }
        }

        return true;
    }

    public void Locked()
    {
        foreach (InteractableTrigger trig in triggers)
        {
            trig.isLocked = true;
        }
    }
}

//Needs to go on any triggerable
public class InteractableManager : MonoBehaviour
{
    [Header("Variables Needed")]
    //The triggerable it's managing
    [SerializeField]
    private MonoBehaviour triggerable;
    //The states of triggers to trigger/untrigger it
    [SerializeField]
    private List<TriggerState> triggerStates = new List<TriggerState>();
    private ITriggerable itriggerable;

    private void Awake()
    {
        itriggerable = (ITriggerable)triggerable;
    }

    //Checking all the trigger states for the triggerable
    public void CheckTriggerStates()
    {
        foreach (TriggerState state in triggerStates)
        {
            if (state.CheckIfTriggered())
            {
                //Checking that there's an itriggerable
                if (itriggerable != null)
                {
                    //Triggering it
                    itriggerable.Triggered();
                }
                //If there's a monobehaviour
                else if (triggerable)
                {
                    if (Debug.isDebugBuild)
                    {
                        Debug.Log("The monoobehaviour given is not an ITriggerable", this);
                    }

                    //But if it's an interactable trigger
                    if (triggerable.GetType().IsSubclassOf(typeof(InteractableTrigger)))
                    {
                        //UnLock it
                        InteractableTrigger NewTriggerable = (InteractableTrigger)triggerable;
                        NewTriggerable.isLocked = false;
                    }
                }

                if (state.bTriggerLock)
                {
                    state.Locked();                  
                }

                return;
            }
        }

        itriggerable.UnTriggered();
    }
}
