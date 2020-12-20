////////////////////////////////////////////////////////////
// File: IProjectileModifier.cs
// Author: Charles Carter
// Brief: Interface for anything that changes the projectile
////////////////////////////////////////////////////////////

using UnityEngine;

public interface IProjectileModifier
{
    void ActivateProjectileEffect(GameObject playerWhoFired, float startSpeed, WeaponProjectile projectileUsed);
    void OnProjectileHit(Collision2D collision);
}
