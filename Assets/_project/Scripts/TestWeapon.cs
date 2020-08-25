using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWeapon : MonoBehaviour, IWeapon
{
    void IWeapon.Fire() => Fire();
    void IWeapon.Pickup() => Pickup();
    void IWeapon.Reload() => Reload();
    void IWeapon.Throw(Vector3 dir) => Throw(dir);
    GameObject IWeapon.ReturnObject() => ReturnThisGameObject();
    bool IWeapon.CanPickup() => canPickup;

    [Header("Components Needed")]

    [SerializeField]
    BoxCollider2D _collider;
    [SerializeField]
    Rigidbody2D rb;

    [Header("Weapon Customization")]

    [SerializeField]
    private float throwForce;

    [HideInInspector]
    public bool canPickup;

    private void Awake()
    {
        _collider = _collider ?? transform.GetChild(0).GetComponent<BoxCollider2D>();
        rb = rb ?? GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        canPickup = true;
        if (throwForce == 0)
        {
            throwForce = 20f;
        }
    }

    private void Fire()
    {

    }

    private void Pickup()
    {
        _collider.enabled = false;
    }

    private void Reload()
    {

    }

    private void Throw(Vector3 dir)
    {
        rb.AddForce(dir * throwForce, ForceMode2D.Impulse);
        _collider.enabled = true;
        StartCoroutine(pickupCooldown(1f));
    }

    private GameObject ReturnThisGameObject()
    {
        return gameObject;
    }

    private IEnumerator pickupCooldown(float timer)
    {
        canPickup = false;

        for (float t = 0; t < timer; t += Time.deltaTime)
        {
            yield return null;
        }

        canPickup = true;
    }
}
