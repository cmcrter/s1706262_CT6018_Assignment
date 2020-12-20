////////////////////////////////////////////////////////////
// File: HomingProjectile.cs
// Author: Charles Carter
// Brief: Projectiles that home on the closest damageable target
////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HomingProjectile : MonoBehaviour, IProjectileModifier
{
    #region Interface Contracts

    void IProjectileModifier.ActivateProjectileEffect(GameObject playerWhoFired, float startSpeed, WeaponProjectile proj) => ProjecileEffect(playerWhoFired, startSpeed);
    void IProjectileModifier.OnProjectileHit(Collision2D collision) => ThisProjectileHit(collision);

    #endregion

    #region Class Variables

    [Header("Variables Needed For Homing Effect")]
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private float homingSpeed;
    [SerializeField]
    private float rotationSpeed;
    private GameObject player;
    private bool bCollided;
    private float fTimeBeforeHoming = 0.1f;
    private float initialProjSpeed;
    [SerializeField]
    private float fMaxHomingDist = 10f;

    #endregion

    //The projectile's effect
    public void ProjecileEffect(GameObject playerWhoFired, float initialSpeed)
    {
        initialProjSpeed = initialSpeed;
        player = playerWhoFired;

        FindClosestViableTarget();

        if (target)
        {
            StartCoroutine(Co_HomeToTarget(initialSpeed));
        }
    }

    //Finding the closest target to the player
    private void FindClosestViableTarget()
    {
        //Getting all of the damageables in the scene
        IEnumerable<IDamagable> ss = FindObjectsOfType<MonoBehaviour>().OfType<IDamagable>();
        List<MonoBehaviour> potentialTargets = new List<MonoBehaviour>();

        //Go through them and see if they could be a target
        foreach (MonoBehaviour s in ss)
        {
            if (s != (MonoBehaviour)player.GetComponent<IDamagable>())
            {
                potentialTargets.Add(s);
            }
        }

        //Temp variables for distance
        float currentDistance = float.PositiveInfinity;
        Transform newTar = null;

        //Go through the remaining potentials target
        foreach (MonoBehaviour pTarget in potentialTargets)
        {
            //Seeing if it's applicable
            if (pTarget.enabled && pTarget.gameObject.activeSelf)
            {
                //Distance checking
                float distToCheck = Vector3.Distance(pTarget.transform.position, transform.position);

                if (distToCheck < currentDistance)
                {
                    currentDistance = distToCheck;
                    newTar = pTarget.transform;
                }
            }
        }

        target = newTar;

        //If the closest viable target is outside the max range
        if (currentDistance > fMaxHomingDist)
        {
            //The there's no viable target
            target = null;
        }
    }
    
    //The actual homing effect
    private IEnumerator Co_HomeToTarget(float startSpeed)
    {
        //homingSpeed = startSpeed;
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Vector3 deltaPosition = homingSpeed * direction * Time.deltaTime;

        //Give it a small cooldown before it starts homing
        yield return new WaitForSeconds(fTimeBeforeHoming);

        //Whilst this projectile has not collided with something
        while (!bCollided)
        {
            //And it has a target, rotate towards them
            if (target)
            {
                //Turn to correct direction based on target
                direction = (Vector2)target.position - rb.position;
                direction.Normalize();
                float rotateAmount = Vector3.Cross(direction, transform.right).z;
                rb.angularVelocity = -rotationSpeed * rotateAmount;
            }
            else
            {
                rb.angularVelocity = 0;
            }

            //Travel in the direction currently set, target or not
            rb.velocity = transform.right * homingSpeed * Time.deltaTime;

            //Because it's moving the rigid body it should be with the physics updates
            yield return new WaitForFixedUpdate();
        }
    }

    //Letting the projectile know to stop homing 
    public void ThisProjectileHit(Collision2D collision2D)
    {
        bCollided = true;
        //This is just ending the homing not anything else
    }
}
