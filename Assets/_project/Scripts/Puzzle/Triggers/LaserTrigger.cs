using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrigger : InteractableTrigger, IMirrorable
{
    void IMirrorable.Hit(Vector3 direction, RaycastHit2D mirrorHit, LaserWeapon weapon, int ID, LayerMask mask) => Hit();

    void Hit()
    {
        isActivated = true;
        CheckTriggered();
    }
}
