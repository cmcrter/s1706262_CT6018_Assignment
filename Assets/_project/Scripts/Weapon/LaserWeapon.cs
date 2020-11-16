using UnityEngine;

public class LaserWeapon : Weapon
{
    private InputHandler handler;

    [Header("Laser Weapon Specific")]

    [SerializeField]
    private LineRenderer lRenderer;
    [SerializeField]
    private ParticleSystem VFX;
    private GameObject laserHit;

    [SerializeField]
    private GameObject barrell;

    [SerializeField]
    private float damageperSecond = 30f;

    [SerializeField]
    private LayerMask mask;
    RaycastHit2D hit;

    public int iMaxBounces = 5;
    public int iBounces = 0;

    private void Update()
    {
        if (handler)
        {
            if (!handler.FireWeapon() && lRenderer.enabled)
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
        //Resetting the laser
        UpdateLaser();

        //Seeing if the laser hit
        OnLaserHit(laserHit);
    }

    public void OnLaserHit(GameObject thislaserHit)
    {
        if (thislaserHit)
        {
            //It damages if it can
            if (thislaserHit.TryGetComponent(out IDamagable damagable))
            {
                //Taking the damage for this tick
                damagable.Damage(damageperSecond * Time.deltaTime);
            }
            //Nothing damagable will also be a mirror (for now)
            else if (thislaserHit.TryGetComponent(out MirrorPanel mirror) && iBounces < iMaxBounces)
            {
                mirror.ShowReflection((hit.point - new Vector2(transform.position.x, transform.position.y)).normalized, hit.normal.normalized, lRenderer, this);
            }
        }
    }

    private void EnableLaser()
    {
        lRenderer.enabled = true;
    }

    private void UpdateLaser()
    {
        lRenderer.SetPosition(0, barrell.transform.position);
        Vector3 EndPoint = RayCastHit();
        EndPoint = new Vector3(EndPoint.x, EndPoint.y, 0);

        iBounces = 0;

        //Resetting all the points to the end of the current laser    
        for (int i = 1;  i < 7; ++i)
        {
            lRenderer.SetPosition(i, EndPoint);
        }
    }

    private void DisableLaser()
    {
        lRenderer.enabled = false;
    }

    private Vector3 RayCastHit()
    {
        hit = Physics2D.Linecast(transform.position, transform.position + (transform.right * 35), mask, -1, 1);

        if (hit)
        {
            laserHit = hit.transform.gameObject;
            return hit.point;
        }

        laserHit = null;
        return transform.position + (transform.right * 35);
    }
}
