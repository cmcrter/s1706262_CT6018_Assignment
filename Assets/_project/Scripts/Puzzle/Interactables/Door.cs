////////////////////////////////////////////////////////////
// File: Door.cs
// Author: Charles Carter
// Brief: A script to handle the functionality of a door
////////////////////////////////////////////////////////////

using UnityEngine;

//This is a door
public class Door : MonoBehaviour, ITriggerable
{
    #region Interface Contracts

    void ITriggerable.Triggered()   => DoorOpened();
    void ITriggerable.UnTriggered() => DoorClosed();
    void ITriggerable.Locked()      => DoorLocked();
    void ITriggerable.Unlocked()    => DoorUnlocked();
    bool ITriggerable.GetLockState() => isDoorLocked();

    #endregion

    #region Class Variables

    [Header("Variables Needed for Door")]
    [SerializeField]
    private Collider2D _collider;
    private MeshRenderer _meshRend;
    [SerializeField]
    private Material doorOpenedMat;
    [SerializeField]
    private Material doorClosedMat;
    [SerializeField]
    private bool bTriggerLock = false;
    private bool bLocked = false;

    #endregion

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _meshRend = GetComponent<MeshRenderer>();
    }

    private void DoorClosed()
    {
        if (bLocked) return;
        _collider.enabled = true;

        if (!doorClosedMat) return;
        _meshRend.material = doorClosedMat;
    }

    private void DoorOpened()
    {
        //Guard Clause
        if (bLocked) return;

        _collider.enabled = false;

        if (doorOpenedMat)
        {
            _meshRend.material = doorOpenedMat;
        }

        if (bTriggerLock)
        {
            DoorLocked();
        }
    }

    // "Locked" refers to the trigger of opening or closing the door
    private void DoorLocked()
    {
        bLocked = true;
    }

    private void DoorUnlocked()
    {
        bLocked = false;
    }

    private bool isDoorLocked()
    {
        return bLocked;
    }
}
