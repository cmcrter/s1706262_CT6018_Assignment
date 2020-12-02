using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    InputHandler handler;

    bool bCurrentlySwinging = false;

    private void Start()
    {
        bCanPickup = true;
        isCurrentlyHeld = false;
    }

    //Overriding the fire function
    public override void FireWeapon(GameObject playerWhoShot, InputHandler inputTypeUsed)
    {
        handler = inputTypeUsed;

        if (bCanFire)
        {
            //Figure out a way to start a jab in the current direction...
        }
    }
}
