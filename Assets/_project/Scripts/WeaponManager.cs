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
                ThrowWeapon();
                currentWeapon.Throw();
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
            currentWeapon = GetClosestWeapon();
            currentWeapon.Pickup();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Picking up the weapon if the player doesnt have one
        if (collision.TryGetComponent<IWeapon>(out var weapon))
        {
            if (!hasWeapon)
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
    private IWeapon GetClosestWeapon()
    {
        IWeapon closestWeapon = weaponsInRange[0];
        float dist = Vector3.Distance(this.transform.position, closestWeapon.ReturnObject().transform.position);

        for (int i = 1; i < weaponsInRange.Count; ++i)
        {
            float distToCheck = Vector3.Distance(this.transform.position, weaponsInRange[i].ReturnObject().transform.position);

            if (distToCheck < dist)
            {
                dist = distToCheck;
                closestWeapon = weaponsInRange[i];
            }
        }

        return closestWeapon;
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

    private void ThrowWeapon()
    {
        hasWeapon = false;
    }

    private void FireWeapon()
    {

    }
}
