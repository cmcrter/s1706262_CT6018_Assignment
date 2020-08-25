using UnityEngine;

public interface IWeapon
{
    void Pickup();
    void Fire();
    void Throw(Vector3 dir);
    void Reload();

    GameObject ReturnObject();
    bool CanPickup();
}
