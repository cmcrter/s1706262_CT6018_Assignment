using System.Collections;
using UnityEngine;

public class ButtonTrigger : InputTrigger
{
    [Header("Button Specific Variables")]
    //Variables for general button functionality
    [SerializeField]
    GameObject buttonObject;
    [SerializeField]
    float fButtonCooldown;

    //Dealing with material changed 
    [SerializeField]
    Material pressedMaterial;
    Material startMaterial;
    [SerializeField]
    CState state;
    [SerializeField]
    Renderer _renderer;

    private void Awake()
    {
        buttonObject = buttonObject ?? gameObject;
        _renderer = _renderer ?? GetComponent<Renderer>();
        state = state ?? GetComponent<CState>();
    }

    public override void InputTriggered()
    {
        base.InputTriggered();
        StartCoroutine(eButtonCooldown());
    }

    private IEnumerator eButtonCooldown()
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
