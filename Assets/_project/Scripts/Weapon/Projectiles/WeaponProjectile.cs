using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The projectile made by weapons
public class WeaponProjectile : MonoBehaviour
{
    [Header("Component projectiles need")]
    [SerializeField]
    Rigidbody2D _rb;
    [SerializeField]
    Collider2D _collider;

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

    //The projectile can have multiple effects
    IProjectileModifier[] projectileEffect;

    private void Awake()
    {
        _rb = _rb ?? GetComponent<Rigidbody2D>();
        _collider = _collider ?? GetComponent<Collider2D>();

        projectileEffect = projectileEffect ?? GetComponents<IProjectileModifier>();
    }

    //When the object is instaniated
    private void OnEnable()
    {
        _rb.gravityScale = 0;
        _collider.enabled = false;
    }

    //When it gets fired
    public virtual void Fired(GameObject PlayerWhoShot, Vector3 dir)
    {
        //Firing the projectile
        _rb.AddForce(dir * fPower, ForceMode2D.Impulse);
        playerWhoShotThis = PlayerWhoShot;
        destroyTimer = StartCoroutine(Co_destroyCheck());
        gameObject.layer = playerWhoShotThis.layer;

        //There is atleast one projectile effect
        if (projectileEffect.Length > 0)
        {
            foreach (IProjectileModifier modifier in projectileEffect)
            {
                modifier.ActivateProjectileEffect(playerWhoShotThis, fPower, this);
            }
        }
    }

    //When it collides with something else
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Shouldnt care about the player who shot it
        if (collision.gameObject.Equals(playerWhoShotThis))
        {
            return;
        }

        if (projectileEffect.Length > 0)
        {
            foreach (IProjectileModifier modifier in projectileEffect)
            {
                modifier.OnProjectileHit(collision);
            }
        }

        Debug.Log("Projectile hit something");
        OnHit(collision);
              
        if (bDestroyOnHit)
        {
            StopCoroutine(destroyTimer);
            Destroy(gameObject);
        }
    }

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
}
