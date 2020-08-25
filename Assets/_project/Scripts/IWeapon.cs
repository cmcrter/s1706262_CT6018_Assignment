using UnityEngine;

public interface IWeapon
{
    void Pickup();
    void Fire();
    void Throw();
    void Reload();

    GameObject ReturnObject();
}
