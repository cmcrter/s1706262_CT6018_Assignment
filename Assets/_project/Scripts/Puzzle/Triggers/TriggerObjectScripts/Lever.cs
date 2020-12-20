////////////////////////////////////////////////////////////
// File: Lever.cs
// Author: Charles Carter
// Brief: A script for the levers specifically
////////////////////////////////////////////////////////////

using UnityEngine;

//A lever is a type of input trigger
public class Lever : InputTrigger
{
    #region Class Variables

    [Header("Variables for the Lever")]
    [SerializeField]
    private GameObject handle;
    [SerializeField]
    private Transform pivot;

    #endregion

    private void Awake()
    {
        handle = handle ?? transform.GetChild(0).gameObject;
    }

    //Overriding the base functionality
    public override void InputTriggered()
    {
        base.InputTriggered();

        if (handle)
        {
            //It was just activated
            if (isActivated)
            {
                pivot.transform.RotateAround(transform.position, Vector3.forward, -10);
            }
            //It was just unactivated
            else
            {
                pivot.transform.RotateAround(transform.position, Vector3.forward, 10);
            }
        }
    }
}
