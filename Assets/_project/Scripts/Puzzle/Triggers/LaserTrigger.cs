////////////////////////////////////////////////////////////
// File: LaserTrigger.cs
// Author: Charles Carter
// Brief: The trigger that gets activated when hit by a laser
////////////////////////////////////////////////////////////

using UnityEngine;

//Inherits from the overal interactable trigger functionality
public class LaserTrigger : InteractableTrigger, IMirrorable
{
    #region Interface Contracts

    void IMirrorable.Hit(Vector3 direction, RaycastHit2D mirrorHit, LaserWeapon weapon, int ID, LayerMask mask) => Hit();

    #endregion

    private void Hit()
    {
        isActivated = true;
        CheckTriggered();
    }
}
