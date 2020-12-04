using UnityEngine;

public class MirrorPanel : MonoBehaviour, ITriggerable, IMirrorable
{
    #region Interface Contracts

    //Mirror panels can move
    void ITriggerable.Triggered() => OnTriggered();
    void ITriggerable.UnTriggered() => OnUnTriggered();

    void ITriggerable.Locked() => Lock();
    void ITriggerable.Unlocked() => UnLock();

    bool ITriggerable.GetLockState() => ReturnLocked();

    //Reflection when hit
    void IMirrorable.Hit(Vector3 InDirection, RaycastHit2D mirrorHit, LaserWeapon weapon, int ID, LayerMask Mask) => ShowReflection(InDirection, mirrorHit, weapon, ID, Mask);

    #endregion

    #region Variables Needed

    Vector3 currentNormal;

    Vector3 mirrorHitPoint;
    Vector2 newDir;

    [SerializeField]
    GameObject mirroredHit;
    [SerializeField]
    Vector3 normal;

    RaycastHit2D initialHit;
    RaycastHit2D mirrorRayHit;

    //A line renderer on a child object each for each player's/player colour's potential laser
    [SerializeField]
    private LineRenderer[] lineRenderers = new LineRenderer[4];
    private bool[] setDetails = new bool[4];
    //5th is for gameworld lasers, not laser weapons
    [SerializeField]
    private LayerMask[] mask = new LayerMask[5];
    private int[] mirrorID = new int[4];

    #endregion

    public void ShowReflection(Vector3 InDirection, RaycastHit2D mirrorHit, LaserWeapon weapon, int ID, LayerMask newMask)
    {
        if (!lineRenderers[ID].enabled)
        {
            lineRenderers[ID].enabled = true;
            CopyLineRendererDetails(ID, weapon.returnLaser());
        }

        if (!setDetails[ID])
        {
            CopyLineRendererDetails(ID, weapon.returnLaser());
            setDetails[ID] = true;
        }

        //Setting the hit to be the one passed through
        initialHit = mirrorHit;
        mask[ID] = newMask;

        //Calculating the next ray
        Vector3 NextPos = BouncingRay(InDirection, ID);

        //If this is a weapon and it added to the weapons list correctly
        if (weapon && weapon.AddMirror(NextPos, this))
        {
            SetLinePositions(ID, initialHit.point, NextPos);

            //It did hit something after the bounce
            if (mirrorRayHit)
            {
                //Seeing what it hits after the bounce
                weapon.OnLaserHit(mirroredHit, mirrorRayHit, mirrorHit.point);
            }
        }
    }

    private void SetLinePositions(int ID, Vector3 startPos, Vector3 NextPos)
    {
        lineRenderers[ID].SetPosition(0, startPos);
        lineRenderers[ID].SetPosition(1, NextPos);
    }

    private void CopyLineRendererDetails(int ID, LineRenderer newValues)
    {
        lineRenderers[ID].startWidth = newValues.startWidth;
        lineRenderers[ID].endWidth = newValues.endWidth;

        lineRenderers[ID].colorGradient = newValues.colorGradient;

        lineRenderers[ID].startColor = newValues.startColor;
        lineRenderers[ID].endColor = newValues.endColor;

        lineRenderers[ID].material = newValues.material;
    }

    //Disable a laser of a particular player/ player colour
    public void DisableLaser(int ID)
    {
        setDetails[ID] = false;
        SetLinePositions(ID, Vector3.zero, Vector3.zero);
        lineRenderers[ID].enabled = false;
    }

    //Calculating the next point of the ray (from mirror point to what the ray hits after reflected)
    private Vector3 BouncingRay(Vector3 InDirection, int ID)
    {
        newDir = Vector3.Reflect(InDirection, initialHit.normal);

        //Getting whatever is hit next
        Vector3 vHit = RayCastHit(newDir, ID);

        //The end position of the ray now
        vHit = new Vector3(vHit.x, vHit.y, 0);

        return vHit;
    }

    private Vector3 RayCastHit(Vector2 newDir, int ID)
    {
        //ray casting from the hit point along the reflected direction
        mirrorRayHit = Physics2D.Raycast(initialHit.point + initialHit.normal * 0.01f, newDir, 25f, mask[ID], -1);

        //Debug.DrawRay(initialHit.point, initialHit.normal);

        //If it hit an object
        if (mirrorRayHit)
        {
            mirroredHit = mirrorRayHit.transform.gameObject;
            //Debug.Log("Hit: " + mirroredHit.name);

            return mirrorRayHit.point;
        }

        //It hit nothing but return a point in the correct direction
        mirroredHit = null;
        return initialHit.point + (initialHit.normal * 0.01f) + (newDir * 25f);
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
