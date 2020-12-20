////////////////////////////////////////////////////////////
// File: WeaponManager.cs
// Author: Charles Carter
// Brief: The manager class that controls the player weapons
////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class needs to handle input
public class WeaponManager : aHandlesInput
{
    #region Class Variables

    [Header("Components Needed")]
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private PlayerHand hand;
    [SerializeField]
    private BlockingManager blockingManager;

    [Header("Weapon Tracking")]
    [SerializeField]
    private List<Weapon> weaponsInRange = new List<Weapon>();
    [SerializeField]
    private Weapon currentWeapon = null;
    [SerializeField]
    private GameObject currentWeaponObject;
    private bool hasWeapon = false;

    [Header("Needed Objects")]
    [SerializeField]
    private Camera mainCamera;
    private Rigidbody2D currentweaponrb;
    private float angleToMousePos;

    #endregion

    private void Awake()
    {
        mainCamera = mainCamera ?? Camera.main;
        inputHandler = inputHandler ?? gameObject.GetComponentInChildren<KeyAndMouseHandler>();
        _rb = _rb ?? GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        UpdateHandPostion();

        if (hasWeapon && currentWeapon != null)
        {

            //If the player wants to fire the weapon
            if (inputHandler.FireWeapon())
            {
                currentWeapon.FireWeapon(gameObject, inputHandler);
            }

            //If the player wants to throw the weapon
            if (inputHandler.ThrowWeapon())
            {
                ThrowCurrentWeapon();
            }
        }
    }

    private void FixedUpdate()
    {
        if (hasWeapon && currentWeapon != null)
        {
            //Aiming and moving the weapon using physics
            UpdateWeaponCarrying();
        }
    }

        private void LateUpdate()
    {
        if (!hasWeapon && weaponsInRange.Count > 0)
        {
            Weapon weaponToCheck = GetClosestEquippableWeapon();

            if (weaponToCheck != null && weaponToCheck.bCanPickup)
            {
                PickingUpWeapon(weaponToCheck);
            }
        }

        //Object was destroyed in the frame
        if (hasWeapon && currentWeapon == null && currentweaponrb == null)
        {
            ResetWeapon();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Picking up the weapon if the player doesnt have one
        if (collision.TryGetComponent<Weapon>(out var weapon))
        {
            //Debug.Log("On top of weapon: " + weapon.gameObject.name + " can it be picked up? " + weapon.bCanPickup, this);

            if (!hasWeapon && weapon.bCanPickup)
            {
                PickingUpWeapon(weapon);
            }
            else
            {
                weaponsInRange.Add(weapon);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Picking up the weapon if the player doesnt have one
        if (collision.TryGetComponent<Weapon>(out var weapon))
        {
            weaponsInRange.Remove(weapon);
        }
    }

    private void PickingUpWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
        currentWeaponObject = weapon.ReturnWeapon();

        currentweaponrb = currentWeapon.GetRB();
        UpdateWeaponCarrying();

        blockingManager.OnWeaponPickup();
        hasWeapon = true;
        weapon.PickupWeapon(gameObject);
    }

    //Getting the closest weapon to the player
    private Weapon GetClosestEquippableWeapon()
    {
        Weapon tempWeapon = null;

        float dist = Mathf.Infinity;

        for (int i = 0; i < weaponsInRange.Count; ++i)
        {
            if (weaponsInRange[i].bCanPickup)
            {
                float distToCheck = Vector3.Distance(transform.position, weaponsInRange[i].ReturnWeapon().transform.position);

                if (distToCheck < dist)
                {
                    dist = distToCheck;
                    tempWeapon = weaponsInRange[i];
                }
            }
        }

        //This should be fine
        if (tempWeapon != null)
        {
            return tempWeapon;
        }

        return null;
    }

    //Getting the correct values from the player hand
    private void UpdateHandPostion()
    {
        hand.UpdateHandObjectPos();
        angleToMousePos = hand.angleToCursorPos;
    }

    //Moving the weapon to correct spot and rotation
    private void UpdateWeaponCarrying()
    {
        currentweaponrb.MoveRotation(angleToMousePos);
        currentweaponrb.MovePosition(Vector3.Lerp(currentweaponrb.position, hand.handPos, Time.deltaTime * 100f));   
        
        //If the weapon is too far away (it can get caught on objects)
        if (Vector3.Distance(currentweaponrb.transform.position, transform.position) > 5f)
        {
            //Move it to the hand
            currentweaponrb.transform.position = hand.handPos;
        }
    }

    private void ThrowCurrentWeapon()
    {
        weaponsInRange.Remove(currentWeapon);
        hasWeapon = false;
        currentWeapon.ThrowWeapon();
        currentweaponrb = null;
        currentWeapon = null;
        blockingManager.OnWeaponDrop();
    }

    public void ResetWeapon()
    {
        hasWeapon = false;

        currentweaponrb = null;
        currentWeapon = null;
        blockingManager.OnWeaponDrop();
    }
}
