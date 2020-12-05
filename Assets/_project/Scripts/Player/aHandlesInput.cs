using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aHandlesInput : MonoBehaviour
{
    [Header("The current type of input")]
    [SerializeField]
    protected InputHandler inputHandler;

    [SerializeField]
    protected KeyAndMouseHandler keyAndMouse;

    private void Awake()
    {
        inputHandler = inputHandler ?? keyAndMouse;
    }

    public void SetInputHandler(InputHandler newHandler)
    {
        inputHandler = newHandler;
    }

    public InputHandler GetInputHandler()
    {
        return inputHandler;
    }
}
