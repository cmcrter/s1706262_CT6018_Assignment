using System.Collections;
using UnityEngine;

//This is a weapon that fires a projectile
public class ProjectileWeapon : Weapon
{
    [Header("Projectile Weapon Specific")]
    [SerializeField]
    private float fProjectileSpeed;
    [SerializeField]
    private float fFireRate = 0.25f;
    [SerializeField]
    private bool bAutoFire = false;
    [SerializeField]
    private GameObject goProjectilePrefab;

    private void Start()
    {
        bCanPickup = true;
        isCurrentlyHeld = false;
    }

    public override void FireWeapon()
    {
        if (bCanFire)
        {
            Debug.Log("Fired test weapon", this);
            StartCoroutine(Co_ShotCooldown());
        }
    }

    IEnumerator Co_ShotCooldown()
    {
        bCanFire = false;

        for (float t = 0; t < fFireRate; t += Time.deltaTime)
        {
            yield return null;
        }

        bCanFire = true;

        if (bAutoFire && Input.GetMouseButton(0) && isCurrentlyHeld)
        {
            FireWeapon();
        }
    }
}
