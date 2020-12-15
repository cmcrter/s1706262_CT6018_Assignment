using System;
using UnityEngine;

public abstract class InputHandler : MonoBehaviour
{
    //This turns false every frame, but any controls from an input type changes it
    protected bool bBeingUsed = false;
    //This is the playerID based on using the controls, not player colour etc
    [SerializeField]
    protected int playerID = 0;

    //Checking through all the controls at the end of every frame to see if it was used
    private void LateUpdate()
    {
        if (MoveLeft() || MoveRight() || Jump() || Crouch() || FireWeapon() || ThrowWeapon() || Interact() || Block() || AimLeft() || AimRight() || AimUp() || AimDown())
        {
            bBeingUsed = true;
        }
        else
        {
            bBeingUsed = false;
        }
    }

    //A return function to see if this input handler is being used
    public bool isBeingUsed()
    {
        return bBeingUsed;
    }

    //General Controls
    public virtual bool MoveLeft() { return false; }
    public virtual bool MoveRight() { return false; }
    public virtual bool Jump() { return false; }
    public virtual bool Crouch() { return false; }
    public virtual bool FireWeapon() { return false; }
    public virtual bool ThrowWeapon() { return false; }
    public virtual bool Interact() { return false; }
    public virtual bool Block() { return false; }

    //For controllers specifically
    public virtual bool AimLeft() { return false; }
    public virtual bool AimRight() { return false; }

    public virtual bool AimUp() { return false; }
    public virtual bool AimDown() { return false; }

    public virtual int GetPlayerID() { return 1; }

    //UI Controls
    public virtual Tuple<bool, int> ToggleMenu() { return new Tuple<bool, int>(false, 0); }
}
