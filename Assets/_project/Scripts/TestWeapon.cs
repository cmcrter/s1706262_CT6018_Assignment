using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWeapon : MonoBehaviour, IWeapon
{
    void IWeapon.Fire() => Fire();
    void IWeapon.Pickup() => Pickup();
    void IWeapon.Reload() => Reload();
    void IWeapon.Throw() => Throw();
    GameObject IWeapon.ReturnObject() => ReturnThisGameObject();

    [SerializeField]
    BoxCollider2D _collider;
    [SerializeField]
    Rigidbody2D rb;

    private void Fire()
    {

    }

    private void Pickup()
    {
        rb.isKinematic = true;
        _collider.enabled = false;
    }

    private void Reload()
    {

    }

    private void Throw()
    {

    }

    private GameObject ReturnThisGameObject()
    {
        return gameObject;
    }
}
