using UnityEngine;

public class MirrorPanel : MonoBehaviour, ITriggerable
{
    #region Interface Contracts

    void ITriggerable.Triggered() => OnTriggered();
    void ITriggerable.UnTriggered() => OnUnTriggered();

    void ITriggerable.Locked() => Lock();
    void ITriggerable.Unlocked() => UnLock();

    bool ITriggerable.GetLockState() => ReturnLocked();

    #endregion

    #region Variables Needed

    Vector3 currentNormal;

    [SerializeField]
    private LayerMask mask;
    Vector3 mirrorHitPoint;
    Vector3 newDir;

    [SerializeField]
    GameObject mirroredHit;
    [SerializeField]
    Vector3 normal;

    RaycastHit2D initialHit;
    RaycastHit2D mirrorRayHit;

    #endregion

    public void ShowReflection(Vector3 InDirection, RaycastHit2D mirrorHit, LaserWeapon weapon)
    {
        //Setting the hit to be the one passed through
        initialHit = mirrorHit;

        //Calculating the next ray
        Vector3 NextPos = BouncingRay(InDirection, mirrorHit);

        //If this is a weapon (later, an interface)
        if (weapon)
        {
            //It did hit something after the bounce
            if (mirrorRayHit)
            {
                //Adding the end position to the weapon
                weapon.AddMirror(NextPos);

                //Seeing what it hits after the bounce
                weapon.OnLaserHit(mirroredHit, mirrorRayHit, NextPos);
            }
        }
    }

    //Calculating the next point of the ray (from mirror point to what the ray hits after reflected)
    private Vector3 BouncingRay(Vector3 InDirection, RaycastHit2D mirrorHit)
    {
        newDir = Vector3.Reflect(InDirection, mirrorHit.normal);

        //Getting whatever is hit next
        Vector3 vHit = RayCastHit(newDir);

        //The end position of the ray now
        vHit = new Vector3(vHit.x, vHit.y, 0);

        return vHit;
    }

    private Vector3 RayCastHit(Vector3 newDir)
    {
        mirrorHitPoint = initialHit.point;
        //Line casting from the hit point along the reflected direction
        mirrorRayHit = Physics2D.Linecast(initialHit.point + initialHit.normal * 0.05f, mirrorHitPoint + (newDir * 15), mask, -1, 1);

        Debug.DrawRay(initialHit.point, initialHit.normal);
        //Debug.DrawRay(initialHit.point, mirrorHitPoint + (newDir * 15));

        //If it hit an object
        if (mirrorRayHit)
        {
            mirroredHit = mirrorRayHit.transform.gameObject;
            return mirrorRayHit.point;
        }

        //It hit nothing but return a point in the correct direction
        mirroredHit = null;
        return mirrorHitPoint + (newDir * 35);
    }

    #region Interface Functions

    //Triggering and untriggering 
    private void OnTriggered()
    {

    }

    private void OnUnTriggered()
    {

    }

    //Locking and unlocking it
    private void Lock()
    {

    }

    private void UnLock()
    {

    }

    //Seeing whether it's locked or not
    public bool ReturnLocked()
    {
        return false;
    }

    #endregion
}
