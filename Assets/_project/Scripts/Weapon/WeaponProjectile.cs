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
    private float fPower = 15f;

    Coroutine destroyTimer;
    [SerializeField]
    GameObject playerWhoShotThis;

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
        //Small chance but just incase
        if (dir == Vector3.zero)
        {
            dir = Vector3.right;
        }

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

        StopCoroutine(destroyTimer);
        Destroy(gameObject);
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
}
