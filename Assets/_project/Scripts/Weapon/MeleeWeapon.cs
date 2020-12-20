////////////////////////////////////////////////////////////
// File: MeleeWeapon.cs
// Author: Charles Carter
// Brief: A weapon which is melee based
////////////////////////////////////////////////////////////

using UnityEngine;

//Inherits from the weapon class 
public class MeleeWeapon : Weapon
{
    private InputHandler handler;
    private bool bCurrentlySwinging = false;

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
            //ToDo: Jab the weapon in a direction
        }
    }
}
