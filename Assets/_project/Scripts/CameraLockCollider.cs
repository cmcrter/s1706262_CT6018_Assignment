////////////////////////////////////////////////////////////
// File: CameraLockCollider.cs
// Author: Charles Carter
// Brief: A collider for the area to lock the camera
//////////////////////////////////////////////////////////// 

using UnityEngine;

//Needs a collider to function
[RequireComponent(typeof(Collider2D))]
public class CameraLockCollider : MonoBehaviour
{
    #region Class Variables

    [Header("Variables Needed")]
    [SerializeField]
    private Camera cameraUsed;
    private CameraMovement _camMovement;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Movement2D playerMovement;
    [SerializeField]
    private float newCamSize;

    #endregion

    private void Awake()
    {
        cameraUsed = cameraUsed ?? Camera.main;
        _camMovement = cameraUsed.GetComponent<CameraMovement>();
        player = player ?? GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Checking whether it's the player
        if (collision.gameObject.Equals(player))
        {
            //Making sure the movement is set
            if (_camMovement != null)
            {
                //Having the camera locked here
                _camMovement.OverrideCameraPos(transform.position, newCamSize, playerMovement);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Checking whether it's the player
        if (collision.gameObject.Equals(player))
        {
            //Making sure the movement is set
            if (_camMovement != null)
            {
                //Freeing the camera
                _camMovement.FreeCameraMovement();
            }
        }
    }
}
