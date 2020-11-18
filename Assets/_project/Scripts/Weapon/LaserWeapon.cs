using System.Collections.Generic;
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

    private int iMaxBounces = 5;
    private int panelCounter = 0;

    private List<Vector2> mirrorHits;

    private void Awake()
    {
        mirrorHits = new List<Vector2>();
    }

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
        //Making sure laser is on
        EnableLaser();
        handler = inputTypeUsed;

        //VFX.Play();
        //Resetting the laser
        UpdateLaser();

        //Seeing if the laser hit
        OnLaserHit(laserHit, hit, transform.position);

        //Clearing the current mirrors for next frame
        mirrorHits.Clear();
    }

    //The Laser's end point has hit something
    public void OnLaserHit(GameObject thislaserHit, RaycastHit2D thisHit, Vector3 LaserPosition)
    {
        //Null check
        if (thislaserHit)
        {
            //It damages if it can
            if (thislaserHit.TryGetComponent(out IDamagable damagable))
            {
                //Taking the damage for this tick
                damagable.Damage(damageperSecond * Time.deltaTime);
            }
            //Nothing damagable will also be a mirror (for now)
            else if (thislaserHit.TryGetComponent(out IMirrorable mirror) && mirrorHits.Count < iMaxBounces)
            {
                //The indirection of the reflection
                Vector3 direction = (thisHit.point - new Vector2(LaserPosition.x, LaserPosition.y)).normalized;

                //The mirror calculates what it hits next etc
                mirror.Hit(direction, thisHit, this);
            }
        }
    }

    private void EnableLaser()
    {
        lRenderer.enabled = true;

        //Resetting the bounce number before the mirror calculations
        mirrorHits.Clear();
    }

    private void UpdateLaser()
    {
        lRenderer.SetPosition(0, barrell.transform.position);
        Vector3 EndPoint = RayCastHit();
        EndPoint = new Vector3(EndPoint.x, EndPoint.y, 0);

        //Resetting all the points to the end of the current laser    
        for (int i = 1;  i < 7; ++i)
        {
            lRenderer.SetPosition(i, EndPoint);
        }
    }

    private void DisableLaser()
    {
        mirrorHits.Clear();
        lRenderer.enabled = false;
    }

    private Vector3 RayCastHit()
    {
        hit = Physics2D.Raycast(transform.position, transform.right, 35, mask, -1);

        if (hit)
        {
            laserHit = hit.transform.gameObject;
            return hit.point;
        }

        laserHit = null;
        return transform.position + (transform.right * 35);
    }

    public void AddMirror(Vector2 EndPos, MirrorPanel panel)
    {
        //If the current hit mirrors is above the max bounce or the line renderer doesnt have space for it
        if (mirrorHits.Count >= iMaxBounces || 2 + iMaxBounces > lRenderer.positionCount)
        {
            //back out
            return;
        }

        //Adding this mirror to the hits list, the hits count will never be 0
        mirrorHits.Add(EndPos);

        //Going through the mirrors and setting points correctly
        for (int i = 0; i < mirrorHits.Count; ++i)
        {
            lRenderer.SetPosition(2 + i, mirrorHits[i]);
        }

        ///Not hit the maximum amount of bounces
        if (mirrorHits.Count < iMaxBounces)
        {
            //Filling in the rest of the positions in the line rendered
            for (int i = mirrorHits.Count; i < iMaxBounces; ++i)
            {
                lRenderer.SetPosition(2 + i, mirrorHits[mirrorHits.Count - 1]);
            }
        }

        //Debug.Log("Mirror Count: " + mirrorHits.Count + " End pos: " + mirrorHits[mirrorHits.Count - 1]);
    }
}
