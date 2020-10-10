using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XboxControllerHandler : InputHandler
{
    [SerializeField]
    int playerID = 1;

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
}
