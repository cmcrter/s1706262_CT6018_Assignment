using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : InputTrigger
{
    [SerializeField]
    GameObject buttonObject;

    [SerializeField]
    float fButtonCooldown;

    private void Awake()
    {
        buttonObject = buttonObject ?? gameObject;
    }

    public override void InputTriggered()
    {
        base.InputTriggered();
        StartCoroutine(eButtonCooldown());
    }

    private IEnumerator eButtonCooldown()
    {
        isLocked = true;

        yield return new WaitForSeconds(fButtonCooldown);

        isLocked = true;
    }
}
