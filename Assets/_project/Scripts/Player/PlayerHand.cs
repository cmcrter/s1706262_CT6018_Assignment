using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField]
    Camera mainCamera;

    [SerializeField]
    GameObject goHandObject;

    //Registering the inputs to move the cursor if controller based
    [SerializeField]
    InputHandler currentInput;

    //Controller player requires a virtual cursor
    [SerializeField]
    Transform tCursor;

    [SerializeField]
    bool bControllerPlay;

    float fCursorBounds = 5f;

    //Variables that scripts using the hand might need
    public float angleToCursorPos { private set; get; }
    public Quaternion handRotation { private set; get; }

    private void Update()
    {
        UpdateCursorObjectPos();
    }

    private void UpdateCursorObjectPos()
    {
        //If it's a controller moving the hand
        if (bControllerPlay)
        {
            //Right joystick to move virual cursor vertically and horizontally 
            if (currentInput.AimLeft() && (tCursor.position.x - transform.position.x) > fCursorBounds)
            {
                tCursor.position += new Vector3(-1, 0);
            }
            if (currentInput.AimRight() && (tCursor.position.x + transform.position.x) > fCursorBounds)
            {
                tCursor.position += new Vector3(+1, 0);
            }

            if (currentInput.AimUp() && (tCursor.position.y + transform.position.y) > fCursorBounds)
            {
                tCursor.position += new Vector3(0, +1);
            }
            if (currentInput.AimDown() && (tCursor.position.y - transform.position.y) > fCursorBounds)
            {
                tCursor.position += new Vector3(0, -1);
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
        goHandObject.transform.position = transform.position + new Vector3(0, 0.75f, 0) + dir.normalized;
    }

    //The input handler could change
    public void SetInputHandler(InputHandler newInputHandler)
    {
        tCursor.position = transform.position;
        currentInput = newInputHandler;
    }
}
