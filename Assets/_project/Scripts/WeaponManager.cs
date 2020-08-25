using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    List<IWeapon> weaponsInRange = new List<IWeapon>();

    [SerializeField]
    IWeapon currentWeapon;
    [SerializeField]
    GameObject currentWeaponObject;
    bool hasWeapon = false;

    [SerializeField]
    Transform playerHandPoint;
    [SerializeField]
    Camera mainCamera;

    private void Awake()
    {
        playerHandPoint = playerHandPoint ?? transform.GetChild(1);
        mainCamera = mainCamera ?? Camera.main;
    }

    private void Update()
    {
        if (hasWeapon)
        {
            UpdateWeaponDirection();

            //If the player wants to throw the weapon and there's a weapon to throw
            if (Input.GetKey(KeyCode.Q))
            {
                ThrowWeapon(currentWeaponObject.transform.right);
            }

            if (Input.GetMouseButton(0))
            {
                currentWeapon.Fire();
            }
        }
    }

    private void LateUpdate()
    {
        if (!hasWeapon && weaponsInRange.Count > 0)
        {
            IWeapon weaponToCheck = GetClosestEquippableWeapon();

            if (weaponToCheck != null && weaponToCheck.CanPickup())
            {
                currentWeapon = weaponToCheck;
                currentWeapon.Pickup();
                hasWeapon = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Picking up the weapon if the player doesnt have one
        if (collision.TryGetComponent<IWeapon>(out var weapon))
        {
            if (!hasWeapon && weapon.CanPickup())
            {
                currentWeapon = weapon;
                currentWeaponObject = weapon.ReturnObject();

                weapon.Pickup();

                currentWeaponObject.transform.position = playerHandPoint.position;
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
        if (collision.TryGetComponent<IWeapon>(out var weapon))
        {
            weaponsInRange.Remove(weapon);
        }
    }

    //Getting the closest weapon to the player
    private IWeapon GetClosestEquippableWeapon()
    {
        TestWeapon closestWeapon = null;
        IWeapon tempWeapon = closestWeapon;

        float dist = Mathf.Infinity;

        for (int i = 0; i < weaponsInRange.Count; ++i)
        {
            if (weaponsInRange[i].CanPickup())
            {
                float distToCheck = Vector3.Distance(this.transform.position, weaponsInRange[i].ReturnObject().transform.position);

                if (distToCheck < dist)
                {
                    dist = distToCheck;
                    tempWeapon = weaponsInRange[i];
                }
            }
        }

        //This should be fine
        if ((TestWeapon)tempWeapon != closestWeapon)
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

    private void ThrowWeapon(Vector3 dir)
    {
        currentWeapon.Throw(dir);
        hasWeapon = false;
        currentWeapon = null;
    }

    private void FireWeapon()
    {

    }
}
