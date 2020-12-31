////////////////////////////////////////////////////////////
// File: LaserWeapon.cs
// Author: Charles Carter
// Brief: A weapon which fires a raycast and displays as a laser
////////////////////////////////////////////////////////////

using System.Collections.Generic;
using UnityEngine;

public class LaserWeapon : Weapon
{
    #region Class Variables

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
    private float fMaxLaserRange = 100f;

    [SerializeField]
    //The possible layer masks to avoid killing the player
    private LayerMask[] masks = new LayerMask[4];
    private int playerID = 0;
    private RaycastHit2D hit;
    private int iMaxBounces = 5;
    private int panelCounter = 0;
    private List<MirrorPanel> mirrorHits;

    #endregion

    private void Awake()
    {
        mirrorHits = new List<MirrorPanel>();
    }

    private void Update()
    {
        if (handler)
        {
            if (!handler.FireWeapon() && lRenderer.enabled)
            {
                ClearMirrors();
                DisableLaser();
            }
        }
    }

    #region Private Class Functions

    private void EnableLaser()
    {
        lRenderer.enabled = true;

        //Wiping the mirrors away to calculate this frame
        ClearMirrors();
    }

    private void UpdateLaser()
    {
        lRenderer.SetPosition(0, barrell.transform.position);
        lRenderer.SetPosition(1, RayCastHit());
    }

    private void DisableLaser()
    {        
        lRenderer.enabled = false;
    }

    private void ClearMirrors()
    {
        foreach (MirrorPanel mirror in mirrorHits)
        {
            mirror.DisableLaser(playerID);
        }

        mirrorHits.Clear();
    }

    private Vector3 RayCastHit()
    {
        hit = Physics2D.Raycast(transform.position, transform.right, fMaxLaserRange, masks[playerID], -1);

        if (hit)
        {
            laserHit = hit.transform.gameObject;
            return hit.point;
        }

        laserHit = null;
        return transform.position + (transform.right * fMaxLaserRange);
    }

    #endregion

    #region Public Class Functions

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
    }

    //The Laser's end point has hit something
    public void OnLaserHit(GameObject thislaserHit, RaycastHit2D thisHit, Vector3 LaserPosition)
    {
        //Null checking the hits
        if (thislaserHit && thisHit)
        {
            //It damages if it can
            if (thislaserHit.TryGetComponent(out IDamagable damagable))
            {
                //Taking the damage for this tick
                damagable.Damage(damageperSecond * Time.deltaTime);
            }
            //Nothing damagable will also be a mirror (for now)
            if (thislaserHit.TryGetComponent(out IMirrorable mirror) && mirrorHits.Count < iMaxBounces)
            {
                //The indirection of the reflection
                Vector3 direction = (thisHit.point - new Vector2(LaserPosition.x, LaserPosition.y)).normalized;

                //The mirror calculates what it hits next etc
                mirror.Hit(direction, thisHit, this, playerID, masks[playerID]);
            }
        }
    }

    public bool AddMirror(Vector2 EndPos, MirrorPanel panel)
    {
        //If the current hit mirrors is above the max bounce or the line renderer doesnt have space for it
        if (mirrorHits.Count >= iMaxBounces)
        {
            //back out
            return false;
        }

        mirrorHits.Add(panel);
        return true;
    }

    public override void PickupWeapon(GameObject player)
    {
        base.PickupWeapon(player);

        //I dont want the laser weapons to hit the player that fired it
        playerID = player.GetComponent<CState>().returnID();
    }

    public LineRenderer returnLaser()
    {
        return lRenderer;
    }

    #endregion
}
