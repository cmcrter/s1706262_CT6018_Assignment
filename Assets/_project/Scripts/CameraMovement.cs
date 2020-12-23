////////////////////////////////////////////////////////////
// File: CameraMovement.cs
// Author: Charles Carter
// Brief: The movement for a camera object 
//////////////////////////////////////////////////////////// 

using System.Collections;
using UnityEngine;

//The camera's movement during singleplayer, requires a camera to do
[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    #region Variables

    [Header("Variables Needed")]
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private Transform ObjectToFollow;
    public bool isLocked;
    [SerializeField]
    private Vector3 offset;
    private Vector3 camZBeforeLock;
    private float defaultCamSize;

    #endregion

    private void Awake()
    {
        cam = cam ?? Camera.main;
    }

    private void Start()
    {
        if (offset.Equals(Vector3.zero))
        {
            offset = transform.position - ObjectToFollow.position;

            if (Debug.isDebugBuild)
            {
                Debug.Log("No offset set in inspector", this);
            }
        }

        //A small cooldown before the follow enables
        StartCoroutine(Co_timeBeforeCameraFollow(0.02f));
        defaultCamSize = cam.orthographicSize;
    }

    private void Update()
    {
        //If the camera isnt locked, move it to follow the target with the offset
        if (!isLocked)
        {
            transform.position = new Vector3(ObjectToFollow.position.x + offset.x, ObjectToFollow.position.y + offset.y, -20);
        }
    }


    #region Private Class Functions

    private IEnumerator Co_timeBeforeCameraFollow(float timer)
    {
        //Locking the camera for a certain amount of time
        isLocked = true;
        yield return new WaitForSeconds(timer);
        isLocked = false;
    }

    private IEnumerator Co_MoveCamerToPoint(Vector3 newPos, float timer, MonoBehaviour tempLockScript)
    {
        //If there's a script moving the camera needs to lock
        if (tempLockScript)
        {
            //Turn it off
            tempLockScript.enabled = false;
        }

        //The actual movement timer
        for (float t = 0; t < timer; t += Time.deltaTime)
        {
            //Smooth transform movement
            transform.position = Vector3.Lerp(transform.position, newPos, t);
            yield return null;
        }

        //If there's a script moving the camera needs to lock
        if (tempLockScript)
        {
            //Turn it back on
            tempLockScript.enabled = true;
        }
    }
    #endregion

    #region Public Variables

    //Public function that overiddes the camera's position and size
    public void OverrideCameraPos(Vector3 newPos, float newCamSize, MonoBehaviour tempLockScripts)
    {
        isLocked = true;
        newPos = new Vector3(newPos.x, newPos.y, transform.position.z);
        cam.orthographicSize = newCamSize;

        StartCoroutine(Co_MoveCamerToPoint(newPos, 1.0f, tempLockScripts));
    }

    public void FreeCameraMovement()
    {
        isLocked = false;
        cam.orthographicSize = defaultCamSize;
        transform.position = new Vector3(transform.position.x, transform.position.y, camZBeforeLock.z);
    }

    #endregion
}