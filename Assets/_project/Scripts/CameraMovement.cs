using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The camera's movement during singleplayer
[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    Camera cam;

    [SerializeField]
    private Transform ObjectToFollow;

    public bool isLocked;

    [SerializeField]
    private Vector3 offset;

    private Vector3 camZBeforeLock;

    float defaultCamSize;

    private void Awake()
    {
        cam = cam ?? Camera.main;
    }

    private void Start()
    {
        if (offset.Equals(Vector3.zero))
        {
            offset = new Vector3(-4, 8, 0);
        }
        StartCoroutine(Co_timeBeforeCameraFollow(0.02f));

        defaultCamSize = cam.orthographicSize;
    }

    private void Update()
    {
        if (!isLocked)
        {
            transform.position = new Vector3(ObjectToFollow.position.x + offset.x, ObjectToFollow.position.y + offset.y, -20);
        }
    }

    private IEnumerator Co_timeBeforeCameraFollow(float timer)
    {
        isLocked = true;

        for (float t = 0; t < timer; t += Time.deltaTime)
        {
            yield return null;
        }

        isLocked = false;
    }

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
}
