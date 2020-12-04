using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingProjectile : MonoBehaviour, IProjectileModifier
{
    #region Interface contracts

    void IProjectileModifier.ActivateProjectileEffect(GameObject playerWhoFired, float startSpeed, WeaponProjectile proj) => ChangeProjectileMaterial(proj);
    void IProjectileModifier.OnProjectileHit(Collision2D collision) => BouncyProjectileHit();

    #endregion

    [Header("Variables To Change")]

    [SerializeField]
    PhysicsMaterial2D bouncyMaterial;

    Rigidbody2D rbToChange;

    //Making it a bouncy projectile
    void ChangeProjectileMaterial(WeaponProjectile projectileToChange)
    {
        projectileToChange.SetDestroyOnHit(false);
        rbToChange = projectileToChange.GetRb();

        rbToChange.sharedMaterial = bouncyMaterial;
    }

    //This should all be done by the physics engine
    void BouncyProjectileHit()
    {

    }
}
