using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrigger : InteractableTrigger, IMirrorable
{
    void IMirrorable.Hit(Vector3 direction, RaycastHit2D mirrorHit, LaserWeapon weapon) => Hit();

    void Hit()
    {
        CheckTriggered();
    }
}
