////////////////////////////////////////////////////////////
// File: aHandlesInput.cs
// Author: Charles Carter
// Brief: Any class that wants to monitor input type
////////////////////////////////////////////////////////////

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
