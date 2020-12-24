////////////////////////////////////////////////////////////
// File: Teleport.cs
// Author: Charles Carter
// Brief: The class for a teleport pad
////////////////////////////////////////////////////////////

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

    #region Class Variables

    [Header("Variables Needed for teleporter")]
    [SerializeField]
    private bool isLocked;
    [SerializeField]
    private bool bLockCamera;
    [SerializeField]
    private CameraMovement cameraToLockMovement;
    [SerializeField]
    private Camera cameraToLock;
    [SerializeField]
    private GameObject SecondTeleportPosition;
    [SerializeField]
    private float fTeleportCooldown;
    [SerializeField]
    private GameObject objectToTeleport;

    //The locking/unlocking sprite changes for local versus
    [SerializeField]
    private SpriteRenderer sRenderer;
    [SerializeField]
    private Sprite LockedSprite;
    [SerializeField]
    private Sprite UnLockedSprite;

    #endregion

    private void Awake()
    {
        ShowLockedSprite();
    }

    //Temporarily lock the teleporter after use
    private IEnumerator Co_TeleportCooldown()
    {
        LockTeleporter();
        yield return new WaitForSeconds(fTeleportCooldown);
        UnLockTeleporter();
    }

    //The function to actually move the player
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
            StartCoroutine(Co_TeleportCooldown());
        }
    }

    private void UnTeleportPlayer()
    {
        //Just to fill interface contract
    }

    private void ShowLockedSprite()
    {
        if (sRenderer)
        {
            if (isLocked)
            {
                sRenderer.sprite = LockedSprite;
            }
            else
            {
                sRenderer.sprite = UnLockedSprite;
            }
        }
    }

    private void LockTeleporter()
    {
        isLocked = true;

        ShowLockedSprite();
    }

    private void UnLockTeleporter()
    {
        isLocked = false;

        ShowLockedSprite();
    }

    public bool GetTeleporterLock()
    {
        return isLocked;
    }
}
