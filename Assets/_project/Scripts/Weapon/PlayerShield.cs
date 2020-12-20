////////////////////////////////////////////////////////////
// File: PlayerShield.cs
// Author: Charles Carter
// Brief: The class which manages the player's shield
////////////////////////////////////////////////////////////

using UnityEngine;

public class PlayerShield : MonoBehaviour, IDamagable
{
    #region Inteface contracts

    void IDamagable.Damage(float amount) => ShieldDamaged(amount);
    void IDamagable.Heal(float amount) => ShieldHealed(amount);

    #endregion

    #region Class Variables
    [Header("Variables Needed for Shield")]
    [SerializeField]
    private PlayerHand hand;
    [SerializeField]
    private BlockingManager manager;
    [SerializeField]
    private float fMaxShieldHealth = 25f;
    [SerializeField]
    private float fCurrentShieldHealth;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        fCurrentShieldHealth = fMaxShieldHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //Using the hand to change the pos and rotation
        if (hand)
        {
            //Pushing the box collider into stuff it might not want to be in, might cause issues at some point, but it's a neat thing to play with
            transform.position = hand.handPos;
            transform.rotation = hand.handRotation;
        }
    }

    //The interface functions for the shield being damaged or healed
    private void ShieldDamaged(float incomingDamage)
    {
        if (gameObject.activeSelf)
        {
            fCurrentShieldHealth -= incomingDamage;

            if (fCurrentShieldHealth <= 0)
            {
                if (manager)
                {
                    manager.ShieldBroke();
                }
                fCurrentShieldHealth = fMaxShieldHealth;
            }
        }
    }

    private void ShieldHealed(float incomingHeal)
    {
        if (gameObject.activeSelf)
        {
            fCurrentShieldHealth += incomingHeal;

            if (fCurrentShieldHealth >= fMaxShieldHealth)
            {
                fCurrentShieldHealth = fMaxShieldHealth;
            }
        }
    }
}
