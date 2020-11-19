using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Something that reacts when hit by mirror
public interface IMirrorable
{
    void Hit(Vector3 direction, RaycastHit2D mirrorHit, LaserWeapon weapon, int ID, LayerMask mask);
}
