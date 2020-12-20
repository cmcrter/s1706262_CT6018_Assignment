////////////////////////////////////////////////////////////
// File: IHoldable.cs
// Author: Charles Carter
// Brief: Interface for anything that can be held
////////////////////////////////////////////////////////////

using UnityEngine;

public interface IHoldable
{
    void Pickup(GameObject player);
    void Throw(Vector3 dir);

    GameObject ReturnObject();
    bool CanPickup();
}
