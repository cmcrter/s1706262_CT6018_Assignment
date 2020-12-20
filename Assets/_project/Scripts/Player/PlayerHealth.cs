////////////////////////////////////////////////////////////
// File: PlayerHealth.cs
// Author: Charles Carter
// Brief: A class to manage a player's health
////////////////////////////////////////////////////////////

using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    #region Interface Contracts

    void IDamagable.Damage(float amount) => DamagedPlayer(amount);
    void IDamagable.Heal(float amount) => HealedPlayer(amount);

    #endregion

    #region Class Variables

    [Header("Variables Needed for health")]
    [SerializeField]
    private Renderer _renderer;
    [SerializeField]
    private Renderer _topRenderer;
    [SerializeField]
    private ParticleSystem deathVFX;
    [SerializeField]
    private CState state;

    [SerializeField]
    private LocalVersusManager versusManager;
    //[SerializeField]
    //SingleplayerManager singleManager;

    [Header("Customization Variables")]
    [SerializeField]
    private float fMaxHealth = 10;
    private float fCurrentHealth;

    #endregion

    private void Awake()
    {
        _renderer = _renderer ?? GetComponent<Renderer>();
        _topRenderer = _topRenderer ?? GetComponentInChildren<Renderer>();
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
        fCurrentHealth = 0;

        return true;
    }

    private void PlayerDeath()
    {
        //Making the player look death and playing the vfx
        _renderer.material.color = Color.gray;
        _topRenderer.material.color = Color.gray;

        var main = deathVFX.main;
        main.startColor = state.GetColor();
        deathVFX.Play();

        //Letting the respective gamemode manager know
        if (versusManager)
        {
            versusManager.PlayerDied(state.returnID());
        }
        //else if (singleManager)
        //{

        //}
    }

    //When players need to be reset
    public void ResetHealth()
    {
        fCurrentHealth = fMaxHealth;

        _renderer.material = state.GetMaterial();
        _topRenderer.material = state.GetMaterial();
    }

    //When players disconnect etc
    public void InstantKillPlayer()
    {
        DamagedPlayer(fMaxHealth);
    }
}
