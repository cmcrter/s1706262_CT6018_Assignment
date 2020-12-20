////////////////////////////////////////////////////////////
// File: IMirrorable.cs
// Author: Charles Carter
// Brief: Interface for anything that reacts to lasers
////////////////////////////////////////////////////////////

using UnityEngine;

//Something that reacts when hit by mirror
public interface IMirrorable
{
    void Hit(Vector3 direction, RaycastHit2D mirrorHit, LaserWeapon weapon, int ID, LayerMask mask);
}
