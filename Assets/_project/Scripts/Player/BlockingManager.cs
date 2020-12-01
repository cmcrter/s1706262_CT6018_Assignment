using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingManager : MonoBehaviour
{
    [SerializeField]
    InputHandler input;
    [SerializeField]
    GameObject goBlockObject;

    public bool bPlayerCanBlock;

    float fShieldRegenTime;

    IEnumerator cooldown;

    private void Awake()
    {
        cooldown = eBlockDestroyedCooldown(fShieldRegenTime);
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
        if (input)
        {
            if (input.Block() && !goBlockObject.activeSelf && bPlayerCanBlock)
            {
                goBlockObject.SetActive(true);
            }

            if (!input.Block() && goBlockObject.activeSelf || !bPlayerCanBlock)
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
    private IEnumerator eBlockDestroyedCooldown(float fSeconds)
    {
        bPlayerCanBlock = false;
        yield return new WaitForSeconds(fSeconds);
        bPlayerCanBlock = true;
    }
}
