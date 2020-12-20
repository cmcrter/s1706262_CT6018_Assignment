////////////////////////////////////////////////////////////
// File: WeaponProjectile.cs
// Author: Charles Carter
// Brief: A projectile that the weapon fires
////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//The projectile made by weapons
public class WeaponProjectile : MonoBehaviour, IWaypointDestructable
{
    #region Interface Contract

    void IWaypointDestructable.Destruct() => Destroy(gameObject);

    #endregion

    #region Class Variables
    [Header("Component projectiles need")]
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private Collider2D _collider;

    [Header("Projectile Customizable Variables")]
    [SerializeField]
    private bool bDestroyOnHit;
    //The speed of the projectile is handled by itself
    [SerializeField]
    private float fPower;
    [SerializeField]
    private float damage;

    private Coroutine destroyTimer;
    [SerializeField]
    private GameObject playerWhoShotThis;
    private Vector3 fireDir;

    //The projectile can have multiple effects
    private List<IProjectileModifier> projectileEffect;

    #endregion

    private void Awake()
    {
        _rb = _rb ?? GetComponent<Rigidbody2D>();
        _collider = _collider ?? GetComponent<Collider2D>();

        projectileEffect = projectileEffect ?? GetComponents<IProjectileModifier>().ToList();
    }

    //When the object is instaniated
    private void OnEnable()
    {
        _rb.gravityScale = 0;
        _collider.enabled = false;
    }

    //When it collides with something else
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Shouldnt care about the player who shot it
        if (collision.gameObject.Equals(playerWhoShotThis))
        {
            return;
        }

        //If there's projectile modifiers
        if (projectileEffect.Count > 0)
        {
            //Go through them
            foreach (IProjectileModifier modifier in projectileEffect)
            {
                //Do the modifier hit function
                modifier.OnProjectileHit(collision);
            }
        }

        //Debug.Log("Projectile hit something");
        OnHit(collision);

        if (bDestroyOnHit)
        {
            StopCoroutine(destroyTimer);
            Destroy(gameObject);
        }
    }

    #region Class Private Functions

    //This is so projectiles that are out the map or can bounce dont stay infinitely
    private IEnumerator Co_destroyCheck()
    {
        _collider.enabled = true;

        for (float t = 0; t < 10f; t += Time.deltaTime)
        {
            yield return null;
        }

        Destroy(gameObject);
    }

    protected virtual void OnHit(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Rigidbody2D>(out var otherRB))
        {
            otherRB.AddForce(transform.forward * fPower, ForceMode2D.Impulse);
        }

        if (collision.gameObject.TryGetComponent<IDamagable>(out var damagable))
        {
            damagable.Damage(damage);
        }
    }

    #endregion

    #region Public Class Functions

    //When it gets fired
    public virtual void Fired(GameObject PlayerWhoShot, Vector3 dir)
    {
        //Firing the projectile
        _rb.AddForce(dir * fPower, ForceMode2D.Impulse);
        playerWhoShotThis = PlayerWhoShot;
        destroyTimer = StartCoroutine(Co_destroyCheck());
        gameObject.layer = playerWhoShotThis.layer;

        //Storing the direction the projectile was fired
        fireDir = dir;

        //There is atleast one projectile effect
        if (projectileEffect.Count > 0)
        {
            foreach (IProjectileModifier modifier in projectileEffect)
            {
                modifier.ActivateProjectileEffect(playerWhoShotThis, fPower, this);
            }
        }
    }

    //For changing projectiles to be bouncy
    public void SetDestroyOnHit(bool bNewDestroyOnHit)
    {
        bDestroyOnHit = bNewDestroyOnHit;
    }

    public Rigidbody2D GetRb()
    {
        return _rb;
    }

    //For Split shot projectiles
    public void Clone()
    {

    }

    public void ClearModifiers()
    {
        projectileEffect.Clear();
    }

    public Vector3 GetFireDir()
    {
        return fireDir;
    }

    #endregion
}
