using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPad : MonoBehaviour, ITriggerable
{
    #region Interface Contracts

    void ITriggerable.Triggered() => SwitchColour();
    void ITriggerable.UnTriggered() => ColourSwitched();

    void ITriggerable.Locked() => PadLocked();
    void ITriggerable.Unlocked() => PadUnlocked();

    bool ITriggerable.GetLockState() => PadLockState();

    #endregion

    [Header("Variables Needed For Switching States")]

    [SerializeField]
    ColourLayerManager manager;

    [SerializeField]
    public CState stateToSwitchWith;

    [SerializeField]
    CState state;

    [SerializeField]
    private bool bTriggerLock = false;
    private bool bLocked = false;

    private void Awake()
    {
        state = state ?? GetComponent<CState>();
    }

    private void SwitchColour()
    {
        //Temp storage of this state
        int tempState = state.returnID();

        state.SetState(stateToSwitchWith.returnID(), false);
        stateToSwitchWith.SetState(tempState, true);

        if (bTriggerLock)
        {
            PadLocked();
        }
    }

    private void ColourSwitched()
    {
        if (!bLocked)
        {
            StartCoroutine(padCooldown());
        }
    }

    //Locking and Unlocking the pad
    private void PadLocked()
    {
        bLocked = true;
    }

    private void PadUnlocked()
    {
        bLocked = false;
    }

    private bool PadLockState()
    {
        return bLocked;
    }

    private IEnumerator padCooldown()
    {
        for (float t = 0; t < 1f; t += Time.deltaTime)
        {
            yield return null;
        }

        PadUnlocked();
    }
}
