using UnityEngine;

public class PlayerShield : MonoBehaviour, IDamagable
{
    #region Inteface contracts

    void IDamagable.Damage(float amount) => ShieldDamaged(amount);
    void IDamagable.Heal(float amount) => ShieldHealed(amount);

    #endregion

    [SerializeField]
    PlayerHand hand;

    [SerializeField]
    BlockingManager manager;

    [SerializeField]
    float fMaxShieldHealth = 25f;
    [SerializeField]
    float fCurrentShieldHealth;

    // Start is called before the first frame update
    void Start()
    {
        fCurrentShieldHealth = fMaxShieldHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (hand)
        {
            //Pushing the box collider into stuff it might not want to be in, might cause issues at some point
            transform.position = hand.handPos;
            transform.rotation = hand.handRotation;
        }
    }

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
