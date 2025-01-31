﻿////////////////////////////////////////////////////////////
// File: Movement2D.cs
// Author: Charles Carter
// Brief: A class to handle player movement
////////////////////////////////////////////////////////////

using UnityEngine;

public class Movement2D : aHandlesInput
{
    #region Class Variables

    [Header("Components and Objects Needed")]
    [SerializeField]
    private Transform _thisTransform;
    [SerializeField]
    private Rigidbody2D _thisRB;
    [SerializeField]
    private GameObject TopHalf;

    [Header("Movement variables")]
    [SerializeField]
    private float JumpForce;
    [SerializeField]
    private float WalkSpeed;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private LayerMask playerLayer;

    public bool isGrounded { get; private set; }
    public bool isCrouched { get; private set; }

    //Private variables
    private Vector2 raycastPoint;
    private Vector2 localMidBottomPoint;
    private float baseWalkSpeed;
    private float baseJumpForce;

    #endregion

    private void Awake()
    {
        //Get components are expensive
        _thisTransform = _thisTransform ?? GetComponent<Transform>();
        _thisRB = _thisRB ?? GetComponent<Rigidbody2D>();
        TopHalf = TopHalf ?? transform.GetChild(0).gameObject;
    }

    // Start is called before the first frame update
    private void Start()
    {
        //Making sure the variables are set
        if (playerLayer == 0)
        {
            playerLayer = 1 << 10;
        }

        if (JumpForce == 0)
        {
            JumpForce = 8;
            Debug.Log("Set Jump Force in inspector");
        }

        if (WalkSpeed == 0)
        {
            WalkSpeed = 4;
            Debug.Log("Set Walk Speed in inspector");
        }

        if (maxSpeed == 0)
        {
            maxSpeed = 1;
        }

        baseWalkSpeed = WalkSpeed;
        baseJumpForce = JumpForce;
    }

    // Update is called once per frame
    private void Update()
    {
        _thisRB.velocity = new Vector2(Vector2.ClampMagnitude(_thisRB.velocity, maxSpeed).x, _thisRB.velocity.y);

        MovementCheck();
        JumpCheck();

        //Player should only crouch or jump when on ground
        if (isGrounded)
        {
            CrouchCheck();
        }
        else
        {
            //Player is falling so uncrouch
            if (isCrouched)
            {
                UnCrouch();
            }
        }
    }

    private void FixedUpdate()
    {
        GroundedCheck();
    }

    //Checking if player moved
    private void MovementCheck()
    {
        
        //Left
        if (inputHandler.MoveLeft())
        {
            _thisRB.velocity += Vector2.left * WalkSpeed;
            //_thisRB.AddForce(Vector2.left * WalkSpeed * Time.deltaTime, ForceMode2D.Impulse);
        }

        //Right
        if (inputHandler.MoveRight())
        {
            _thisRB.velocity += -Vector2.left * WalkSpeed;
            //_thisRB.AddForce(-Vector2.left * WalkSpeed * Time.deltaTime, ForceMode2D.Impulse);
        }
    }

    //Checking if player jumped
    private void JumpCheck()
    {
        if (inputHandler.Jump() && !isCrouched && isGrounded)
        {
            _thisRB.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
        }
    }

    //Checking if player crouched or not
    private void CrouchCheck()
    {
        if (inputHandler.Crouch())
        {
            ToggleCrouch();
        }
    }

    //UnCrouching
    private void UnCrouch()
    {
        TopHalf.SetActive(true);
        isCrouched = false;
        WalkSpeed = baseWalkSpeed;
    }

    //Crouching
    private void Crouch()
    {
        TopHalf.SetActive(false);
        isCrouched = true;
        WalkSpeed = WalkSpeed * 0.5f;
    }

    //Toggle Option for crouching
    private void ToggleCrouch()
    {
        isCrouched = !isCrouched;

        if (isCrouched)
        {
            Crouch();
        }
        else
        {
            UnCrouch();
        }
    }

    //Checking if there's a ground below the player
    private void GroundedCheck()
    {
        RaycastHit2D hit;
        //Debug.DrawRay(_thisTransform.transform.position, Vector3.down, Color.cyan, 0.05f);

        if (hit = Physics2D.BoxCast(_thisTransform.transform.position, _thisTransform.lossyScale * 0.8f, 0, Vector2.down, 0.1f, ~playerLayer))
        {
            isGrounded = true;
            return;
        }

        isGrounded = false;
    }
}
