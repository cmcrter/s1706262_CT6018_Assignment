using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour, IDamagable
{
    void IDamagable.Damage(float amount) => death(amount);
    void IDamagable.Heal(float amount) => death(amount);

    [SerializeField]
    ParticleSystem VFX;

    [SerializeField]
    Renderer renderer;

    [SerializeField]
    Collider2D collider;

    [SerializeField]
    float radius = 4f;

    [SerializeField]
    float damageDealt = 100f;

    [SerializeField]
    float explosiveForce = 15f;
    
    void death(float amount)
    {
        renderer.enabled = false;
        collider.enabled = false;

        //The explosives should go off
        VFX.Play();
        StartCoroutine(VFXCheck());
        ApplyExplosiveForce();
        //It'll delete itself when the VFX is done
    }

    void ApplyExplosiveForce()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, radius))
        {
            if (collider.TryGetComponent(out Rigidbody2D rb))
            {
                rb.AddForce((collider.transform.position - transform.position) * explosiveForce);
            }

            if (collider.TryGetComponent(out IDamagable damagable))
            {
                damagable.Damage(damageDealt);
            }
        }
    }

    IEnumerator VFXCheck()
    {
        while (VFX.isPlaying)
        {
            yield return null;
        }

        Destroy(gameObject);
    }
}
