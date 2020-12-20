////////////////////////////////////////////////////////////
// File: Explosive.cs
// Author: Charles Carter
// Brief: An object which can be exploded
////////////////////////////////////////////////////////////

using System.Collections;
using UnityEngine;

public class Explosive : MonoBehaviour, IDamagable
{
    #region Interface Contracts

    void IDamagable.Damage(float amount) => death(amount);
    void IDamagable.Heal(float amount) => death(amount);

    #endregion

    #region Class Variables

    [Header("Variables Needed")]
    [SerializeField]
    private ParticleSystem VFX;
    [SerializeField]
    private Renderer _renderer;
    [SerializeField]
    private Collider2D _collider;
    [SerializeField]
    private float radius = 4f;
    [SerializeField]
    private float damageDealt = 100f;
    [SerializeField]
    private float explosiveForce = 15f;

    #endregion

    private void death(float amount)
    {
        _renderer.enabled = false;
        _collider.enabled = false;

        //The explosives should go off
        VFX.Play();
        StartCoroutine(VFXCheck());
        ApplyExplosiveForce();
        //It'll delete itself when the VFX is done
    }

    private void ApplyExplosiveForce()
    {
        //Getting all the colliders within a circle
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, radius))
        {
            //If it has a rigidbody
            if (collider.TryGetComponent(out Rigidbody2D rb))
            {
                //Add a force away from the barrell
                rb.AddForce((collider.transform.position - transform.position) * explosiveForce);
            }

            //If it could be barrell
            if (collider.TryGetComponent(out IDamagable damagable))
            {
                //Damage it but this explosive's amount
                damagable.Damage(damageDealt);
            }
        }
    }

    //Making sure the VFX finishes before completely removing the object
    private IEnumerator VFXCheck()
    {
        while (VFX.isPlaying)
        {
            yield return null;
        }

        Destroy(gameObject);
    }
}
