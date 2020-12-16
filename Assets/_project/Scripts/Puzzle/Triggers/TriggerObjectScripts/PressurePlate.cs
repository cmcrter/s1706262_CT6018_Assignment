using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : CollisionTrigger
{
    public override void InputTriggered()
    {
        base.InputTriggered();

        //It was just activated
        if (isActivated)
        {

        }
        //It was just unactivated
        else
        {

        }
    }
}
