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

    [Header("Variables Needed For Homing Effect")]

    [SerializeField]
    Transform target;
    [SerializeField]
    Rigidbody2D rb;
    [SerializeField]
    float homingSpeed;
    [SerializeField]
    float rotationSpeed;
    GameObject player;
    bool bCollided;
    float fTimeBeforeHoming = 0.1f;
    float initialProjSpeed;

    [SerializeField]
    float fMaxHomingDist = 10f;

    public void ProjecileEffect(GameObject playerWhoFired, float initialSpeed)
    {
        initialProjSpeed = initialSpeed;
        player = playerWhoFired;

        FindClosestViableTarget();

        if (target)
        {
            StartCoroutine(eHomeToTarget(initialSpeed));
        }
    }

    //Finding the closest target to the player
    private void FindClosestViableTarget()
    {
        var ss = FindObjectsOfType<MonoBehaviour>().OfType<IDamagable>();
        List<MonoBehaviour> potentialTargets = new List<MonoBehaviour>();

        foreach (MonoBehaviour s in ss)
        {
            if (s != (MonoBehaviour)player.GetComponent<IDamagable>())
            {
                potentialTargets.Add(s);
            }
        }

        float currentDistance = float.PositiveInfinity;
        Transform newTar = null;

        foreach (MonoBehaviour pTarget in potentialTargets)
        {
            if (pTarget.enabled && pTarget.gameObject.activeSelf)
            {
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
    IEnumerator eHomeToTarget(float startSpeed)
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
