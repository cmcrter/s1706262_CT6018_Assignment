using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D _rb;

    [Header("Weapon Tracking")]

    [SerializeField]
    List<Weapon> weaponsInRange = new List<Weapon>();

    [SerializeField]
    Weapon currentWeapon = null;
    [SerializeField]
    GameObject currentWeaponObject;
    bool hasWeapon = false;

    [Header("Needed Objects")]
    [SerializeField]
    private InputHandler inputHandler;
    [SerializeField]
    Transform playerHandPoint;
    [SerializeField]
    Camera mainCamera;
    //[SerializeField]
    //CharacterManager _manager;
    Quaternion handRotation;
    Rigidbody2D currentweaponrb;
    float angleToMousePos;

    private void Awake()
    {
        playerHandPoint = playerHandPoint ?? transform.GetChild(1);
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
                currentWeapon.FireWeapon(currentWeapon.gameObject, inputHandler);
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Picking up the weapon if the player doesnt have one
        if (collision.TryGetComponent<Weapon>(out var weapon))
        {
            Debug.Log("On top of weapon: " + weapon.gameObject.name + " can it be picked up? " + weapon.bCanPickup, this);

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

        //Expensive but I dont know how else to do this
        currentweaponrb = currentWeapon.GetRB();
        UpdateWeaponCarrying();

        weapon.PickupWeapon();
        hasWeapon = true;
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

    //Moving the hand to the correct transform details
    private void UpdateHandPostion()
    {
        Vector3 dir = Input.mousePosition - mainCamera.WorldToScreenPoint(transform.position);
        angleToMousePos = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        handRotation = Quaternion.AngleAxis(angleToMousePos, Vector3.forward);
        playerHandPoint.position = transform.position + new Vector3(0, 0.75f, 0) + dir.normalized;
    }

    //Moving the weapon to correct spot and rotation
    private void UpdateWeaponCarrying()
    {
        currentweaponrb.MoveRotation(angleToMousePos);
        currentweaponrb.MovePosition(Vector3.Lerp(currentweaponrb.position, playerHandPoint.position, Time.deltaTime * 25f));       
    }

    private void ThrowCurrentWeapon()
    {
        hasWeapon = false;
        currentWeapon.ThrowWeapon();
        currentweaponrb = null;
        currentWeapon = null;
    }
}
