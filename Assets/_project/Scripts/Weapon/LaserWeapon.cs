using UnityEngine;

public class LaserWeapon : Weapon
{
    private InputHandler handler;

    [SerializeField]
    private LineRenderer renderer;
    [SerializeField]
    private ParticleSystem VFX;
    private GameObject laserHit;

    [SerializeField]
    private GameObject barrell;

    [SerializeField]
    private float damageperSecond = 30f;

    [SerializeField]
    private LayerMask mask;

    private void Awake()
    {

    }

    private void Update()
    {
        if (handler)
        {
            if (!handler.FireWeapon() && renderer.enabled)
            {
                DisableLaser();
            }
        }
    }

    public override void FireWeapon(GameObject playerWhoShot, InputHandler inputTypeUsed)
    {
        EnableLaser();
        handler = inputTypeUsed;

        //VFX.Play();
        UpdateLaser();

        if (laserHit)
        {
            if (laserHit.TryGetComponent(out IDamagable damagable))
            {
                //Taking the damage for this tick
                damagable.Damage(damageperSecond * Time.deltaTime);
            }
        }
    }

    private void EnableLaser()
    {
        renderer.enabled = true;
    }

    private void UpdateLaser()
    {
        renderer.SetPosition(0, barrell.transform.position);
        renderer.SetPosition(1, RayCastHit());
    }

    private void DisableLaser()
    {
        renderer.enabled = false;
    }

    private Vector3 RayCastHit()
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, transform.position + (transform.right * 25), mask, -1, 1);

        if (hit)
        {
            laserHit = hit.transform.gameObject;
            return hit.point;
        }

        laserHit = null;
        return transform.position + (transform.right * 50);
    }
}
