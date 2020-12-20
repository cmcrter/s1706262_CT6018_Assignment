////////////////////////////////////////////////////////////
// File: BlockingManager.cs
// Author: Charles Carter
// Brief: A class to handle the player's blocking
////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingManager : aHandlesInput
{
    #region Class Variables

    public bool bPlayerCanBlock;

    [Header("Variables needed for blocking")]
    [SerializeField]
    private GameObject goBlockObject;
    private float fShieldRegenTime;
    private IEnumerator cooldown;

    #endregion

    private void Awake()
    {
        cooldown = Co_ShieldRegenCooldown(fShieldRegenTime);
    }

    // Start is called before the first frame update
    private void Start()
    {
        bPlayerCanBlock = true;
        fShieldRegenTime = 5f;
    }

    // Update is called once per frame
    private void Update()
    {
        if (inputHandler)
        {
            if (inputHandler.Block() && !goBlockObject.activeSelf && bPlayerCanBlock)
            {
                goBlockObject.SetActive(true);
            }

            if (!inputHandler.Block() && goBlockObject.activeSelf || !bPlayerCanBlock)
            {
                goBlockObject.SetActive(false);
            }
        }       
    }

    public void OnWeaponDrop()
    {
        bPlayerCanBlock = true;
        StopCoroutine(cooldown);
    }

    public void OnWeaponPickup()
    {
        bPlayerCanBlock = false;
        StopCoroutine(cooldown);
    }

    public void ShieldBroke()
    {
        StartCoroutine(cooldown);
    }

    //There's a cooldown before the player can put their shield back up
    private IEnumerator Co_ShieldRegenCooldown(float fSeconds)
    {
        bPlayerCanBlock = false;
        yield return new WaitForSeconds(fSeconds);
        bPlayerCanBlock = true;
    }
}
