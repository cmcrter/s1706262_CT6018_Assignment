using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
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

    private void Awake()
    {
        colliderToSpawnWithin = colliderToSpawnWithin ?? GetComponent<Collider2D>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(eSpawnWeaponLoop());
    }

    private IEnumerator eSpawnWeaponLoop()
    {
        fRandTimer = Random.Range(fRandMin, fRandMax);

        for (float t = 0; t < fRandTimer; t += Time.deltaTime)
        {
            yield return null;
        }

        if (iCurrentWeaponsOnMap < iMaxWeaponsOnMap)
        {
            SpawnWeapon(GetRandomSpotInBounds(colliderToSpawnWithin), RandomWeapon());
        }

        StartCoroutine(eSpawnWeaponLoop());
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
}
