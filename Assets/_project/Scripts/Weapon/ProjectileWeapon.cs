////////////////////////////////////////////////////////////
// File: ProjectileWeapon.cs
// Author: Charles Carter
// Brief: A class specifically for projectile based weapons
////////////////////////////////////////////////////////////

using System.Collections;
using UnityEngine;

//Inherits from the base weapon class
public class ProjectileWeapon : Weapon
{
    #region Class Variables

    [Header("Projectile Weapon Specific")]
    [SerializeField]
    private float fFireRate;
    [SerializeField]
    private bool bAutoFire = false;
    [SerializeField]
    private GameObject goProjectilePrefab;
    private InputHandler handler;
    [SerializeField]
    private Transform barrell;

    #endregion

    private void Awake()
    {
        goProjectilePrefab = goProjectilePrefab ?? (GameObject)Resources.Load("_project/Prefabs/TestProjectile");
    }

    private void Start()
    {
        bCanPickup = true;
        isCurrentlyHeld = false;
    }

    //Shooting cooldown applies on every projectile weapon
    private IEnumerator Co_ShotCooldown(GameObject playerWhoShot)
    {
        bCanFire = false;

        for (float t = 0; t < fFireRate; t += Time.deltaTime)
        {
            yield return null;
        }

        bCanFire = true;

        if (bAutoFire && handler.FireWeapon() && isCurrentlyHeld)
        {
            FireWeapon(playerWhoShot, handler);
        }
    }

    //Overriding the fire function
    public override void FireWeapon(GameObject playerWhoShot, InputHandler inputTypeUsed)
    {
        handler = inputTypeUsed;

        if (bCanFire)
        {
            GameObject proj = Instantiate(goProjectilePrefab, barrell.position, transform.rotation, transform.parent);
            proj.GetComponent<WeaponProjectile>().Fired(playerWhoShot, transform.right);

            //Debug.Log("Fired test weapon", this);
            StartCoroutine(Co_ShotCooldown(playerWhoShot));
        }
    }
}
