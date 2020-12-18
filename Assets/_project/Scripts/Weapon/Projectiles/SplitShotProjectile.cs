using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitShotProjectile : MonoBehaviour, IProjectileModifier
{
    #region Interface Contracts

    void IProjectileModifier.ActivateProjectileEffect(GameObject playerWhoFired, float startSpeed, WeaponProjectile projectileUsed) => OnFired(playerWhoFired, startSpeed, projectileUsed);
    void IProjectileModifier.OnProjectileHit(Collision2D collision) => OnHit();

    #endregion

    [Header("Variables for a split shot")]
    [SerializeField]
    float fSplitAngle = 45;
    [SerializeField]
    int iNumOfShots = 2;

    List<GameObject> clonedShots;

    //Using the projectile to create itself at 2 different angles using the direction it was fired
    void OnFired(GameObject playerWhoFired, float startSpeed, WeaponProjectile projectileUsed)
    {
        //Getting the angle that this bullet should be fired at
        Quaternion qAngle = Quaternion.AngleAxis(-iNumOfShots / 2.0f * fSplitAngle, transform.right) * transform.rotation;
        Quaternion qDelta = Quaternion.AngleAxis(fSplitAngle, transform.right);

       //Spawning the new Bullet
       GameObject clone = Instantiate(projectileUsed.gameObject, transform.position + (0.1f * transform.right), transform.rotation);

       //Clearing the modifiers from it, then firing it
       clone.GetComponent<WeaponProjectile>().ClearModifiers();
       clone.GetComponent<WeaponProjectile>().Fired(playerWhoFired, projectileUsed.GetFireDir());
    }

    void OnHit()
    {
        //This modifier does nothing on hit
    }
}
