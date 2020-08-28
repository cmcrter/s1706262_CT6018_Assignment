using UnityEngine;

public interface IHoldable
{
    void Pickup();
    void Throw(Vector3 dir);

    GameObject ReturnObject();
    bool CanPickup();
}
