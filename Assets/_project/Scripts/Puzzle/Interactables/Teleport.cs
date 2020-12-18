using System.Collections;
using UnityEngine;

public class Teleport : MonoBehaviour, ITriggerable
{
    #region Interface Contracts

    void ITriggerable.Triggered() => TeleportPlayer();
    void ITriggerable.UnTriggered() => UnTeleportPlayer();

    void ITriggerable.Locked() => LockTeleporter();
    void ITriggerable.Unlocked() => UnLockTeleporter();

    bool ITriggerable.GetLockState() => GetTeleporterLock();

    #endregion

    [Header("Variables Needed for teleporter")]
    [SerializeField]
    private bool isLocked;
    [SerializeField]
    private bool bLockCamera;
    [SerializeField]
    CameraMovement cameraToLockMovement;
    [SerializeField]
    Camera cameraToLock;
    [SerializeField]
    private GameObject SecondTeleportPosition;
    [SerializeField]
    private float fTeleportCooldown;
    [SerializeField]
    GameObject objectToTeleport;

    //Temporarily lock the teleporter after use
    private IEnumerator eTeleportCooldown()
    {
        LockTeleporter();
        yield return new WaitForSeconds(fTeleportCooldown);
        UnLockTeleporter();
    }

    private void TeleportPlayer()
    {
        if (!isLocked)
        {
            if (cameraToLock && cameraToLockMovement && bLockCamera)
            {
                MonoBehaviour playerMovement = objectToTeleport.GetComponent<Movement2D>();
                cameraToLockMovement.OverrideCameraPos(cameraToLock.transform.position, cameraToLock.orthographicSize, playerMovement);
            }

            objectToTeleport.transform.position = SecondTeleportPosition.transform.position;
            StartCoroutine(eTeleportCooldown());
        }
    }

    private void UnTeleportPlayer()
    {
        //Just to fill interface contract
    }

    private void LockTeleporter()
    {
        isLocked = true;
    }

    private void UnLockTeleporter()
    {
        isLocked = false;
    }

    public bool GetTeleporterLock()
    {
        return isLocked;
    }
}
