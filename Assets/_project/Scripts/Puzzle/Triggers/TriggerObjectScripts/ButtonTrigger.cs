////////////////////////////////////////////////////////////
// File: ButtonTrigger.cs
// Author: Charles Carter
// Brief: A script to specifically handle buttons
////////////////////////////////////////////////////////////

using System.Collections;
using UnityEngine;

//Buttons are a type of input trigger
public class ButtonTrigger : InputTrigger
{
    #region Class Variables

    [Header("Button Specific Variables")]
    //Variables for general button functionality
    [SerializeField]
    private GameObject buttonObject;
    [SerializeField]
    private float fButtonCooldown;
    //Dealing with material changed 
    [SerializeField]
    private Material pressedMaterial;
    private Material startMaterial;
    [SerializeField]
    private CState state;
    [SerializeField]
    private Renderer _renderer;

    #endregion

    private void Awake()
    {
        buttonObject = buttonObject ?? gameObject;
        _renderer = _renderer ?? GetComponent<Renderer>();
        state = state ?? GetComponent<CState>();
    }

    public override void InputTriggered()
    {
        base.InputTriggered();
        StartCoroutine(Co_ButtonCooldown());
    }

    //A cooldown function to limit button spam
    private IEnumerator Co_ButtonCooldown()
    {
        if (pressedMaterial)
        {
            _renderer.material = pressedMaterial;
        }

        isLocked = true;
        isActivated = true;

        yield return new WaitForSeconds(fButtonCooldown);

        _renderer.material = state.GetMaterial();
        isActivated = false;
        isLocked = false;
    }
}
