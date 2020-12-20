////////////////////////////////////////////////////////////
// File: KeyAndMouseHandler.cs
// Author: Charles Carter
// Brief: The input handler for when the player uses a Keyboard and Mouse
////////////////////////////////////////////////////////////

using System;
using UnityEngine;

public class KeyAndMouseHandler : InputHandler
{
    //Overrides for all the applicable Input handler functions
    public override bool MoveLeft()
    {
        return Input.GetKey(KeyCode.A);
    }

    public override bool MoveRight()
    {
        return Input.GetKey(KeyCode.D);
    }

    public override bool Jump()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    public override bool Crouch()
    {
        return Input.GetKeyDown(KeyCode.LeftControl);
    }

    public override bool FireWeapon()
    {
        return Input.GetMouseButton(0);
    }

    public override bool ThrowWeapon()
    {
        return Input.GetKeyDown(KeyCode.Q);
    }

    public override bool Interact()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    public override bool Block()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }

    public override Tuple<bool, int> ToggleMenu()
    {
        return Tuple.Create(Input.GetKeyDown(KeyCode.Escape), playerID);
    }
}
