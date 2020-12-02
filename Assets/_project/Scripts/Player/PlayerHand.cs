using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : aHandlesInput
{
    [Header("Variables needed for the player's hand")]
    [SerializeField]
    Camera mainCamera;

    [SerializeField]
    GameObject goHandObject;

    //Controller player requires a virtual cursor
    [SerializeField]
    Transform tCursor;

    [SerializeField]
    bool bControllerPlay;

    float fCursorBounds = 2f;

    //Variables that scripts using the hand might need
    public float angleToCursorPos { private set; get; }
    public Quaternion handRotation { private set; get; }
    public Vector3 handPos { private set; get; }

    private void Update()
    {
        ControllerPlayCheck();
        UpdateCursorObjectPos();
    }

    private void ControllerPlayCheck()
    {
        if (inputHandler != keyAndMouse)
        {
            bControllerPlay = true;
        }
        else
        {
            bControllerPlay = false;
        }
    }

    private void UpdateCursorObjectPos()
    {
        //If it's a controller moving the hand
        if (bControllerPlay)
        {
            int playerID = inputHandler.GetPlayerID();
            //Right joystick to move virual cursor vertically and horizontally 
            if (inputHandler.AimLeft() || inputHandler.AimRight() || inputHandler.AimUp() || inputHandler.AimDown())
            {
                float joystickAngle = (Mathf.Atan2(Input.GetAxis("AimHorizontal" + playerID.ToString()) * -1, Input.GetAxis("AimVertical" + playerID.ToString())) * Mathf.Rad2Deg) - 90;

                tCursor.rotation = Quaternion.AngleAxis(joystickAngle, Vector3.forward);
                tCursor.position = transform.position + (tCursor.rotation.normalized * new Vector3(0, 2f, 0));
            }
        }
        //If it's mouse based
        else
        {
            tCursor.position = Input.mousePosition;
        }
    }

    //Moving the hand's object itself
    public void UpdateHandObjectPos()
    {
        Vector3 dir = Vector3.zero;

        if (!bControllerPlay)
        {
            dir = tCursor.position - mainCamera.WorldToScreenPoint(transform.position);
        }
        else
        {
            dir = tCursor.position - transform.position;
        }

        angleToCursorPos = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        handRotation = Quaternion.AngleAxis(angleToCursorPos, Vector3.forward);
        handPos = transform.position + new Vector3(0, 0.75f, 0) + dir.normalized;
        goHandObject.transform.position = handPos;
    }
}
