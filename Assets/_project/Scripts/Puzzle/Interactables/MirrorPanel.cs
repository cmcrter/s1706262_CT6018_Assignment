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
    RaycastHit2D thisHit;
    Vector3 vHit;

    #endregion

    public void ShowReflection(Vector3 InDirection, RaycastHit2D mirrorHit, LineRenderer laser, LaserWeapon weapon)
    {
        thisHit = mirrorHit;

        //Calculating the next ray
        BouncingRay(InDirection, mirrorHit);

        //If this is a weapon (later, an interface)
        if (weapon)
        {
            //Increase the bounces
            weapon.iBounces++;
            Debug.Log(newDir + " " + weapon.iBounces + " " + name);

            //Setting all the points to the current known end
            for (int i = 1 + weapon.iBounces; i < 7; ++i)
            {
                //The current bounce is the last one, so set all positions after it to be the same
                laser.SetPosition(i, vHit);
            }

            //Seeing what it hits after the bounce
            weapon.OnLaserHit(mirroredHit);
        }
    }

    //Calculating the next point of the ray (from mirror point to what the ray hits after reflected)
    private void BouncingRay(Vector3 InDirection, RaycastHit2D mirrorHit)
    {
        newDir = Vector3.Reflect(InDirection, mirrorHit.normal);

        //Getting whatever is hit next
        vHit = RayCastHit(newDir);

        //The end position of the ray now
        vHit = new Vector3(vHit.x, vHit.y, 0);
    }

    private Vector3 RayCastHit(Vector3 newDir)
    {
        //Line casting from the hit point along the reflected direction
        RaycastHit2D hit = Physics2D.Linecast(thisHit.point + thisHit.normal * 0.03f, mirrorHitPoint + newDir * 35, mask, -1, 1);

        //If it hit an object
        if (hit)
        {
            //Saving what it hit if it wasnt this transform
            if (hit.transform != transform)
            {
                mirroredHit = hit.transform.gameObject;
            }

            return hit.point;
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
