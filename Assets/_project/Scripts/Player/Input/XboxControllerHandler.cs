using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XboxControllerHandler : InputHandler
{
    public override bool MoveLeft()
    {
        return Input.GetAxis("MoveHorizontal" + playerID.ToString()) < 0;
    }

    public override bool MoveRight()
    {
        return Input.GetAxis("MoveHorizontal" + playerID.ToString()) > 0;
    }

    public override bool Jump()
    {
        return Input.GetButtonDown("Jump" + playerID.ToString());
    }

    public override bool Crouch()
    {
        return Input.GetButtonDown("Crouch" + playerID.ToString());
    }

    public override bool FireWeapon()
    {
        return Input.GetButtonDown("Fire" + playerID.ToString());
    }

    public override bool ThrowWeapon()
    {
        return Input.GetButtonDown("Throw" + playerID.ToString());
    }

    public override bool Interact()
    {
        return Input.GetButtonDown("Interact" + playerID.ToString());
    }

    public override bool Block()
    {
        return base.Block();
    }

    //For moving the virtual cursor
    public override bool AimLeft()
    {
        return Input.GetAxis("AimHorizontal" + playerID.ToString()) > 0;
    }

    public override bool AimRight()
    {
        return Input.GetAxis("AimHorizontal" + playerID.ToString()) < 0;
    }

    public override bool AimUp()
    {
        return Input.GetAxis("AimVertical" + playerID.ToString()) > 0;
    }

    public override bool AimDown()
    {
        return Input.GetAxis("AimVertical" + playerID.ToString()) < 0;
    }

    public override int GetPlayerID()
    {
        return playerID;
    }

    //UI controls
    public override Tuple<bool, int> ToggleMenu()
    {
        return Tuple.Create(Input.GetButtonDown("PauseButton" + playerID.ToString()), playerID);
    }
}
