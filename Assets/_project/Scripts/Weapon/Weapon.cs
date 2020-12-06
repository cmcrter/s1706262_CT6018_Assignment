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

//Any weapon has this base functionality and can be held by a player but shouldnt go past waypoints
public abstract class Weapon : MonoBehaviour, IHoldable, IWaypointDestructable
{
    #region Interface Contracts

    void IHoldable.Pickup(GameObject player) => PickupWeapon(player);
    void IHoldable.Throw(Vector3 dir) => ThrowWeapon();
    GameObject IHoldable.ReturnObject() => ReturnWeapon();
    bool IHoldable.CanPickup() => bCanPickup;

    void IWaypointDestructable.Destruct() => Destroy(gameObject);

    #endregion

    public bool bCanPickup;
    public bool isCurrentlyHeld;
    protected bool bCanFire = true;
    private bool bDamageOnhit = false;

    [Header("Components Needed For Any Weapon")]

    //Collider that collides with environment
    [SerializeField]
    protected BoxCollider2D _collider;
    //Collider that determines pickups
    [SerializeField]
    protected BoxCollider2D _playerCollider = null;

    [SerializeField]
    protected Rigidbody2D _rb;

    [Header("General Weapon Customization")]
    [SerializeField]
    private float throwForce = 10f;
    [SerializeField]
    private float fThrowDamage;
    [SerializeField]
    protected eWeaponType weaponType { get; private set; }
    [SerializeField]
    private float fPickupCooldownTime = 1.5f;

    private void Awake()
    {
        _rb = _rb ?? GetComponent<Rigidbody2D>();
        _collider = _collider ?? GetComponent<BoxCollider2D>();
    }

    public virtual void PickupWeapon(GameObject player)
    {
        _collider.enabled = true;
        _rb.gravityScale = 0;
        isCurrentlyHeld = true;
        gameObject.layer = player.layer;
        bCanPickup = false;
        _playerCollider.enabled = false;
    }

    public GameObject ReturnWeapon()
    {
        return gameObject;
    }

    public void ThrowWeapon()
    {
        _rb.gravityScale = 1;
        _rb.AddForce(transform.right * throwForce, ForceMode2D.Impulse);

        isCurrentlyHeld = false;
        _playerCollider.gameObject.layer = 8;

        StartCoroutine(Co_StartPickupCooldown());
    }

    public virtual void FireWeapon(GameObject playerWhoShot, InputHandler inputTypeUsed)
    {
        //should be overrided
    }

    private IEnumerator Co_StartPickupCooldown()
    {
        bCanPickup = false;
        bDamageOnhit = true;

        //Doing it this way instead of waitnewseconds to check whether it does damage
        for (float t = 0; t < fPickupCooldownTime; t += Time.deltaTime)
        {
            if (_rb.velocity.magnitude < 4f)
            {
                bDamageOnhit = false;
            }

            yield return null;
        }

        bDamageOnhit = false;
        bCanPickup = true;
        _playerCollider.enabled = true;
    }

    //A weapon might do damage on hit if it was thrown
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (bDamageOnhit && collision.gameObject.TryGetComponent<IDamagable>(out var damagable))
        {
            damagable.Damage(fThrowDamage);
        }
    }

    public Rigidbody2D GetRB()
    {
        return _rb;
    }
}
