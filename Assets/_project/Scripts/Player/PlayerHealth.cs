using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    #region Interface Contracts

    void IDamagable.Damage(float amount) => DamagedPlayer(amount);
    void IDamagable.Heal(float amount) => HealedPlayer(amount);

    #endregion

    [SerializeField]
    private Renderer _renderer;

    #region Variables Needed

    [Header("Customization Variables")]
    [SerializeField]
    [Tooltip("The player's maximum health")]
    float fMaxHealth = 10;
    float fCurrentHealth;

    #endregion

    private void Awake()
    {
        
    }

    private void Start()
    {
        fCurrentHealth = fMaxHealth;
    }

    private void DamagedPlayer(float amount)
    {
        fCurrentHealth -= amount;

        //If the player is dead, affect the needed things
        if (isPlayerDead())
        {
            PlayerDeath();
        }
    }

    private void HealedPlayer(float amount)
    {
        fCurrentHealth += amount;
    }

    private bool isPlayerDead()
    {
        if (fCurrentHealth > 0)
        {
            return false;
        }

        return true;
    }

    private void PlayerDeath()
    {
        _renderer.material.color = Color.gray;
    }
}
