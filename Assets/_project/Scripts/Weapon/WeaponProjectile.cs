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
    [SerializeField]
    private float fPower = 15f;
    [SerializeField]
    private float damage = 1f;

    private Coroutine destroyTimer;
    [SerializeField]
    private GameObject playerWhoShotThis;

    private void Awake()
    {
        _rb = _rb ?? GetComponent<Rigidbody2D>();
        _collider = _collider ?? GetComponent<Collider2D>();
    }

    //When the object is instaniated
    private void OnEnable()
    {
        _rb.gravityScale = 0;
        _collider.enabled = false;
    }

    //When it gets fired
    public void Fired(GameObject PlayerWhoShot, Vector3 dir, float shotPower)
    {
        //Firing the projectile
        _rb.AddForce(dir * shotPower, ForceMode2D.Impulse);
        playerWhoShotThis = PlayerWhoShot;
        destroyTimer = StartCoroutine(Co_destroyCheck());
    }

    //When it collides with something else
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Shouldnt care about the player who shot it
        if (collision.gameObject.Equals(playerWhoShotThis))
        {
            return;
        }

        if (collision.gameObject.TryGetComponent<Rigidbody2D>(out var otherRB))
        {
            otherRB.AddForce(transform.forward * fPower, ForceMode2D.Impulse);
        }

        if (collision.gameObject.TryGetComponent<IDamagable>(out var damagable))
        {
            damagable.Damage(damage);
        }

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

    protected virtual void OnHit()
    {

    }
}
