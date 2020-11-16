using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorPanel : MonoBehaviour
{
    Vector3 currentNormal;

    [SerializeField]
    private LayerMask mask;
    Vector3 mirrorHitPoint;

    [SerializeField]
    GameObject mirroredHit;

    public void ShowReflection(Vector3 hitPoint, Vector3 normal, LineRenderer laser, LaserWeapon weapon)
    {
        weapon.iBounces++;
        mirrorHitPoint = hitPoint;

        Vector3 newDir = Vector3.Reflect(hitPoint, normal);
        Vector3 vHit = RayCastHit(newDir);
        vHit = new Vector3(vHit.x, vHit.y, 0);

        Debug.Log(name + " " + weapon.iBounces);

        for (int i = 1 + weapon.iBounces; i < 7; ++i)
        {
            //The current bounce is the last one, so set all positions after it to be the same
            laser.SetPosition(i, vHit);
        }

        //Seeing what it hits after the bounce
        weapon.OnLaserHit(mirroredHit);
    }

    private Vector3 RayCastHit(Vector3 newDir)
    {
        mirrorHitPoint = mirrorHitPoint + newDir * 0.05f;
        RaycastHit2D hit = Physics2D.Linecast(mirrorHitPoint, mirrorHitPoint + newDir * 35, mask, -1, 1);

        if (hit)
        {
            mirroredHit = hit.transform.gameObject;
            return hit.point;
        }

        mirroredHit = null;
        return mirrorHitPoint + (newDir * 35);
    }
}
