using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, ITriggerable
{
    void ITriggerable.Triggered()   => DoorOpened();
    void ITriggerable.UnTriggered() => DoorClosed();
    void ITriggerable.Locked()      => DoorLocked();
    void ITriggerable.Unlocked()    => DoorUnlocked();

    [SerializeField]
    Collider2D _collider;
    MeshRenderer _meshRend;

    [SerializeField]
    Material doorOpenedMat;
    [SerializeField]
    Material doorClosedMat;

    [SerializeField]
    bool bTriggerLock = false;
    bool bLocked = false;

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
}
