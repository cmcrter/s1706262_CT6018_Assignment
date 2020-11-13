using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//For future expansions, making the weapon with a specific type attached (eg: Gamemodes or maps which only use melee weapons)
public enum eWeaponType
{
    Empty,
    Melee,
    Projectile,
    Laser,

    COUNT
}

//Any weapon has this base functionality and can be held by a player
public abstract class Weapon : MonoBehaviour, IHoldable
{
    void IHoldable.Pickup() => PickupWeapon();
    void IHoldable.Throw(Vector3 dir) => ThrowWeapon(dir);
    GameObject IHoldable.ReturnObject() => ReturnWeapon();
    bool IHoldable.CanPickup() => bCanPickup;

    public bool bCanPickup;
    public bool isCurrentlyHeld;
    protected bool bCanFire = true;

    [Header("Components Needed For Any Weapon")]

    //Collider that collides with environment
    [SerializeField]
    private BoxCollider2D _collider;
    //Collider that determines pickups
    [SerializeField]
    private BoxCollider2D _playerCollider = null;

    [SerializeField]
    private Rigidbody2D _rb;

    [Header("General Weapon Customization")]
    [SerializeField]
    private float throwForce = 10f;
    [SerializeField]
    protected eWeaponType weaponType { get; private set; }
    [SerializeField]
    private float fPickupCooldownTime = 1f;

    private void Awake()
    {
        _rb = _rb ?? GetComponent<Rigidbody2D>();
        _collider = _collider ?? GetComponent<BoxCollider2D>();
    }

    public void PickupWeapon()
    {
        _collider.enabled = true;
        isCurrentlyHeld = true;
        bCanPickup = false;
        _playerCollider.enabled = false;
    }

    public GameObject ReturnWeapon()
    {
        return gameObject;
    }

    public void ThrowWeapon(Vector3 dir)
    {
        _rb.AddForce(dir * throwForce, ForceMode2D.Impulse);
        _playerCollider.enabled = false;
        isCurrentlyHeld = false;


        StartCoroutine(Co_StartPickupCooldown());
    }

    public virtual void FireWeapon(GameObject playerWhoShot, InputHandler inputTypeUsed)
    {
        //should be overrided
    }

    private IEnumerator Co_StartPickupCooldown()
    {
        bCanPickup = false;

        for (float t = 0; t < fPickupCooldownTime; t += Time.deltaTime)
        {
            yield return null;
        }

        bCanPickup = true;
        _playerCollider.enabled = true;
    }
}
