using UnityEngine;

public interface IProjectileModifier
{
    void ActivateProjectileEffect(GameObject playerWhoFired, float startSpeed);
    void OnProjectileHit(Collision2D collision);
}
