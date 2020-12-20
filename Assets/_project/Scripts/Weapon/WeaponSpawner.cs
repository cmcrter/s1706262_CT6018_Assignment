////////////////////////////////////////////////////////////
// File: WeaponSpawner.cs
// Author: Charles Carter
// Brief: A factory to spawn in a weapon type
////////////////////////////////////////////////////////////

using System.Collections;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    #region Class Variables

    [Header("Weapon Instantiation Variables")]
    [SerializeField]
    private GameObject[] WeaponPrefabsToUse;
    [SerializeField]
    private Collider2D colliderToSpawnWithin;
    [SerializeField]
    private Transform WeaponParent;

    [Header("Spawn timer variables")]
    [SerializeField]
    private float fRandMin;
    [SerializeField]
    private float fRandMax;
    private float fRandTimer;
    [SerializeField]
    private int iMaxWeaponsOnMap = 5;
    private int iCurrentWeaponsOnMap = 0;

    #endregion

    private void Awake()
    {
        colliderToSpawnWithin = colliderToSpawnWithin ?? GetComponent<Collider2D>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(Co_SpawnWeaponLoop());
    }

    #region Class Functions

    private IEnumerator Co_SpawnWeaponLoop()
    {
        fRandTimer = Random.Range(fRandMin, fRandMax);

        yield return new WaitForSeconds(fRandTimer);

        if (iCurrentWeaponsOnMap < iMaxWeaponsOnMap)
        {
            SpawnWeapon(GetRandomSpotInBounds(colliderToSpawnWithin), RandomWeapon());
        }

        StartCoroutine(Co_SpawnWeaponLoop());
    }

    private GameObject RandomWeapon()
    {
        int Weapon = Random.Range(0, WeaponPrefabsToUse.Length);

        return WeaponPrefabsToUse[Weapon];
    }

    private Vector3 GetRandomSpotInBounds(Collider2D colliderToSpawnIn)
    {
        Bounds bounds = colliderToSpawnIn.bounds;

        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            0
        );
    }

    private void SpawnWeapon(Vector3 spawnPos, GameObject weaponToSpawn)
    {
        Instantiate(weaponToSpawn, spawnPos,  Quaternion.identity, WeaponParent);
    }

    #endregion
}
