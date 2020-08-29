using System.Collections;
using UnityEngine;

//This is a weapon that fires a projectile
public class ProjectileWeapon : Weapon
{
    [Header("Projectile Weapon Specific")]
    [SerializeField]
    private float fProjectileSpeed = 15f;
    [SerializeField]
    private float fFireRate = 0.25f;
    [SerializeField]
    private bool bAutoFire = false;
    [SerializeField]
    private GameObject goProjectilePrefab;

    private void Awake()
    {
        goProjectilePrefab = goProjectilePrefab ?? (GameObject)Resources.Load("_project/Prefabs/");
    }

    private void Start()
    {
        bCanPickup = true;
        isCurrentlyHeld = false;
    }

    //Overriding the fire function
    public override void FireWeapon(GameObject playerWhoShot)
    {
        if (bCanFire)
        {
            GameObject proj = Instantiate(goProjectilePrefab, transform.position, transform.rotation, null);
            proj.GetComponent<WeaponProjectile>().Fired(playerWhoShot, transform.right, fProjectileSpeed);

            Debug.Log("Fired test weapon", this);
            StartCoroutine(Co_ShotCooldown(playerWhoShot));
        }
    }

    //Shooting cooldown applies on every projectile weapon
    IEnumerator Co_ShotCooldown(GameObject playerWhoShot)
    {
        bCanFire = false;

        for (float t = 0; t < fFireRate; t += Time.deltaTime)
        {
            yield return null;
        }

        bCanFire = true;

        if (bAutoFire && Input.GetMouseButton(0) && isCurrentlyHeld)
        {
            FireWeapon(playerWhoShot);
        }
    }
}
