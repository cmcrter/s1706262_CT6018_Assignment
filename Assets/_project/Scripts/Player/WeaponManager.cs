using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
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

    private void Awake()
    {
        playerHandPoint = playerHandPoint ?? transform.GetChild(1);
        mainCamera = mainCamera ?? Camera.main;
        inputHandler = inputHandler ?? gameObject.GetComponentInChildren<KeyAndMouseHandler>();
    }

    private void Update()
    {
        UpdateHandPostion();

        if (hasWeapon && currentWeapon != null)
        {
            //Aiming the weapon towards the mouse
            UpdateWeaponDirection();

            //If the player wants to fire the weapon
            if (inputHandler.FireWeapon())
            {
                currentWeapon.FireWeapon(currentWeapon.gameObject, inputHandler);
            }

            //If the player wants to throw the weapon and there's a weapon to throw
            if (inputHandler.ThrowWeapon())
            {
                ThrowCurrentWeapon(handRotation.eulerAngles);
            }
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
        UpdateWeaponDirection();

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
                float distToCheck = Vector3.Distance(this.transform.position, weaponsInRange[i].ReturnWeapon().transform.position);

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

    private void UpdateHandPostion()
    {
        Vector3 dir = Input.mousePosition - mainCamera.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        handRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        playerHandPoint.rotation = handRotation;
        playerHandPoint.position = transform.position + new Vector3(0, 0.5f, 0) + dir.normalized * (1 + 0.5f);
    }

    private void UpdateWeaponDirection()
    {
        if (currentWeaponObject && currentweaponrb)
        {
            currentWeaponObject.transform.rotation = handRotation;
            currentweaponrb.MovePosition(Vector3.MoveTowards(currentweaponrb.position, playerHandPoint.position, 1000 * Time.deltaTime));
        }
    }

    private void ThrowCurrentWeapon(Vector3 dir)
    {
        hasWeapon = false;
        currentWeapon.ThrowWeapon(dir);
        currentweaponrb = null;
        currentWeapon = null;
    }
}
