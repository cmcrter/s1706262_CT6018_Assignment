////////////////////////////////////////////////////////////
// File: PressurePlate.cs
// Author: Charles Carter
// Brief: A script for pressure plates specifically
////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//It's a type of collision trigger
public class PressurePlate : CollisionTrigger
{
    public override void InputTriggered()
    {
        base.InputTriggered();

        //It was just activated
        if (isActivated)
        {
            //ToDo: Change Material?
        }
        //It was just unactivated
        else
        {
            //ToDo: Change Material Back?
        }
    }
}
