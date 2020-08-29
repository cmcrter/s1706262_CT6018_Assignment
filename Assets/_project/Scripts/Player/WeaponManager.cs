using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapon Tracking")]

    [SerializeField]
    List<Weapon> weaponsInRange = new List<Weapon>();

    [SerializeField]
    Weapon currentWeapon;
    [SerializeField]
    GameObject currentWeaponObject;
    bool hasWeapon = false;

    [Header("Needed Objects")]

    [SerializeField]
    Transform playerHandPoint;
    [SerializeField]
    Camera mainCamera;
    //[SerializeField]
    //CharacterManager _manager;

    private void Awake()
    {
        playerHandPoint = playerHandPoint ?? transform.GetChild(1);
        mainCamera = mainCamera ?? Camera.main;
        //_manager = _manager ?? GetComponent<CharacterManager>();
    }

    private void Update()
    {
        if (hasWeapon && currentWeapon != null)
        {
            //Aiming the weapon towards the mouse
            UpdateWeaponDirection();

            //If the player wants to fire the weapon
            if (Input.GetMouseButtonDown(0))
            {
                currentWeapon.FireWeapon(gameObject);
            }

            //If the player wants to throw the weapon and there's a weapon to throw
            if (Input.GetKey(KeyCode.Q))
            {
                ThrowCurrentWeapon(currentWeaponObject.transform.right);
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
                currentWeapon = weaponToCheck;
                currentWeapon.PickupWeapon();
                hasWeapon = true;
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
                currentWeapon = weapon;
                currentWeaponObject = weapon.ReturnWeapon();
                currentWeaponObject.transform.position = playerHandPoint.position;

                weapon.PickupWeapon();
                hasWeapon = true;
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

    private void UpdateWeaponDirection()
    {
        Vector3 dir = Input.mousePosition - mainCamera.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

        currentWeaponObject.transform.rotation = q;
        playerHandPoint.transform.position = transform.position + new Vector3(0, 0.5f, 0) + dir.normalized * (1 + 0.5f);
        currentWeaponObject.transform.position = playerHandPoint.transform.position;

    }

    private void ThrowCurrentWeapon(Vector3 dir)
    {
        hasWeapon = false;
        currentWeapon.ThrowWeapon(dir);
        currentWeapon = null;
    }
}
